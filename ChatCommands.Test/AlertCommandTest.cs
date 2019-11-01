using System;
using System.Threading.Tasks;
using Moq;
using Service.Core;
using Xunit;

namespace ChatCommands.Test
{
    public class AlertCommandTest
    {
        public AlertCommandTest()
        {
            HubServiceMock = new Mock<IHubService>();
            ChatServiceMock = new Mock<IChatService>();
            Sut = new AlertCommand(HubServiceMock.Object);
        }

        public Mock<IChatService> ChatServiceMock { get; set; }

        public AlertCommand Sut { get; set; }

        public Mock<IHubService> HubServiceMock { get; set; }

        public class Execute_Method : AlertCommandTest
        {
            [Fact]
            public async Task should_not_throw()
            {
                await Sut.Execute(ChatServiceMock.Object, new CommandArgs());
            }

            [Fact]
            public async Task should_send_alert_on_hub_service()
            {
                var username = "CodingGorilla";
                var args = new CommandArgs { UserName = username };

                HubServiceMock.Setup(x => x.SendAlert(username))
                              .Returns(Task.CompletedTask)
                              .Verifiable();

                await Sut.Execute(ChatServiceMock.Object, args);

                HubServiceMock.Verify();
            }
        }
    }
}
