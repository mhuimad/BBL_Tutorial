using Moq;
using PollyTutorial.Contracts;
using PollyTutorial.Dto;
using PollyTutorial.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PollyTutorial.UnitTests
{
    public class GithubServiceTests
    {
        public GithubServiceTests()
        {
           
        }

        [Fact]
        public async  void RetrieveGitHubUser_ShouldRetrySeveralTimes_WhenException()
        {
            //Arrange
            var githubRepository = new Mock<IGithubRepository>(MockBehavior.Strict);
            githubRepository
                .SetupSequence(x => x.GetUserByUserNameAsync(It.IsAny<string>()))
                .ThrowsAsync(new HttpRequestException())
                .ThrowsAsync(new HttpRequestException())
                .Returns(Task.FromResult(new User()));

            var sut = new GitHubService(githubRepository.Object);

            //Act
            var result = await sut.GetUserByUserNameAsync("mhuimad");

            //Assert
            Assert.NotNull(result);
            githubRepository.Verify(x => x.GetUserByUserNameAsync(It.IsAny<string>()),Times.Exactly(2));


        }
    }
}
