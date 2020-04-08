using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PollyTutorial.Dto;
using PollyTutorial.Contracts;

namespace PollyTutorial.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly IHttpClientFactory _httpFactory;
        private static readonly Random Random = new Random();
        private const int MaxRetries = 3;

        public GitHubService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var client = _httpFactory.CreateClient("GitHub");
            var retriesLeft = MaxRetries;
            User user = null;
            while (retriesLeft > 0)
            {
                try
                {
                    if (Random.Next(1, 3) == 1)
                        throw new HttpRequestException("Fake request exception ");

                    var result = await client.GetAsync($"/users/{userName}");
                    if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                        break;

                    var resultString = await result.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(resultString);
                    break;
                }
                catch (HttpRequestException)
                {
                    retriesLeft--;
                    if (retriesLeft == 0)
                        throw;
                }


            }

            return user;
        }


    }
}
