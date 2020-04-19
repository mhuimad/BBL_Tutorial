using PollyTutorial.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollyTutorial.Contracts
{
    public interface IGitHubService
    {
        Task<User> GetUserByUserNameAsync(string userName);
        Task<IEnumerable<User>> GetUsersByOrgAsync(string orgName);
        Task<bool> CreateUserAsync(User user);

        Task<bool> SaveUserInQueueAsync(User user);



    }
}
