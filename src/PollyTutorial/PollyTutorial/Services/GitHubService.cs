using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PollyTutorial.Dto;
using PollyTutorial.Contracts;
using Polly.Retry;
using Polly;
using Polly.Fallback;

namespace PollyTutorial.Services
{
    public class GitHubService : IGitHubService
    {
        private static readonly Random Random = new Random();
        private const int MaxRetries = 2;
        private const string EXCEPTION_MESSAGE = "Fake request exception";
        private const int ExceptionsAllowedBeforeBreaking = 15;
        private const int DurationOfBreakInMinute = 5;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncPolicy _circuitBreakderPolicy;
        private readonly IGithubRepository _githubRepository;

        public GitHubService(IGithubRepository githubRepository)
        {
            _retryPolicy = Policy.Handle<HttpRequestException>()
                                       .WaitAndRetryAsync(MaxRetries, times => TimeSpan.FromMilliseconds(100 * times));
            _circuitBreakderPolicy = Policy.Handle<Exception>()
                .CircuitBreakerAsync(ExceptionsAllowedBeforeBreaking, TimeSpan.FromMinutes(DurationOfBreakInMinute));

            _githubRepository = githubRepository;

        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                if (Random.Next(1, 3) == 1)
                    throw new HttpRequestException(EXCEPTION_MESSAGE);
                return await _githubRepository.GetUserByUserNameAsync(userName);
            });

        }

        public async Task<IEnumerable<User>> GetUsersByOrgAsync(string orgName)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                return await _githubRepository.GetUsersByOrgAsync(orgName);
            });

        }

        public async Task<bool> CreateUserAsync(User user)
        {
            var fallbackPolicy = Policy.Handle<Exception>().FallbackAsync(async (cancellationToken) => await SaveUserInQueueAsync(user));

            var result = await fallbackPolicy
                .WrapAsync(_retryPolicy)
                .WrapAsync(_circuitBreakderPolicy)
                .ExecuteAsync(async () => await _githubRepository.CreateUserAsync(user));

            return result;
        }

        public async Task<bool> SaveUserInQueueAsync(User user)
        {
            await Task.CompletedTask;
            return true;
        }
    }
}
