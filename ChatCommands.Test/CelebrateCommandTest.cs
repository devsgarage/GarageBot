using System;
using System.Threading.Tasks;
using Moq;
using Service.Core;
using Xunit;

namespace ChatCommands.Test
{
    public class CelebrateCommandTest
    {
        public CelebrateCommandTest()
        {
            HubServiceMock = new Mock<IHubService>();
            ChatServiceMock = new Mock<IChatService>();
            Sut = new CelebrateCommand(HubServiceMock.Object);
        }

        public Mock<IChatService> ChatServiceMock { get; set; }

        public CelebrateCommand Sut { get; set; }

        public Mock<IHubService> HubServiceMock { get; set; }

        public class Execute_Method : CelebrateCommandTest
        {
            [Fact]
            public async Task should_not_throw()
            {
                var args = new CommandArgs { IsBroadcaster = true, UserName = "CodingGorilla" };
                await Sut.Execute(ChatServiceMock.Object, args);
            }

            [Fact]
            public async Task should_not_execute_if_not_broadcaster()
            {
                var args = new CommandArgs { UserName = "CodingGorilla" };

                await Sut.Execute(ChatServiceMock.Object, args);

                HubServiceMock.Verify(x => x.SendCelebration(It.IsAny<string>()), Times.Never);
            }

            [Fact]
            public async Task should_celebrate_with_username_for_broadcaster()
            {
                var args = new CommandArgs { IsBroadcaster = true, UserName = "CodingGorilla" };

                HubServiceMock.Setup(x => x.SendCelebration(args.UserName))
                              .Returns(Task.CompletedTask)
                              .Verifiable();

                await Sut.Execute(ChatServiceMock.Object, args);

                HubServiceMock.Verify(x => x.SendCelebration(args.UserName), Times.Once);
            }
        }
    }
}