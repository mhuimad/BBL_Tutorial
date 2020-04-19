using PollyTutorial.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PollyTutorial.Contracts
{
    public interface IGithubRepository
    {   
        Task<User> GetUserByUserNameAsync(string userName);
        Task<IReadOnlyCollection<User>> GetUsersByOrgAsync(string orgName);

        Task<bool> CreateUserAsync(User user);
    }
}