using Newtonsoft.Json;
using PollyTutorial.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PollyTutorial.Contracts
{
    public class GithubRepository : IGithubRepository
    {

        private readonly IHttpClientFactory _httpFactory;

        public GithubRepository(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            await Task.CompletedTask;
            return  true;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var client = _httpFactory.CreateClient("GitHub");
            var result = await client.GetAsync($"/users/{userName}");
            if (!result.IsSuccessStatusCode)
                return null;

            var resultString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(resultString);
        }


        public async Task<IReadOnlyCollection<User>> GetUsersByOrgAsync(string orgName)
        {
            var client = _httpFactory.CreateClient("GitHub");
            var result = await client.GetAsync($"/orgs/{orgName}");
            if (!result.IsSuccessStatusCode)
                return null;

            var resultString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IReadOnlyCollection<User>>(resultString);


        }


    }
}

