using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PollyTutorial.Contracts;

namespace PollyTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        // GET: api/GitHub/5
        [HttpGet("users/{userName}", Name = "GetUser")]
        public async Task<IActionResult> GetUserByUserNameAsync(string userName)
        {
            var user = await _gitHubService.GetUserByUserNameAsync(userName);
            return user != null ? (IActionResult)Ok(user) : NotFound();
        }

        [HttpGet("orgs/{orgName}", Name = "GetUsersByOrgName")]
        public async Task<IActionResult> GetUsersByOrgNameAsync(string orgName)
        {
            var users = await _gitHubService.GetUsersByOrgAsync(orgName);
            return users != null && users.Any() ? (IActionResult)Ok(users) : NotFound();
        }

    }


}
