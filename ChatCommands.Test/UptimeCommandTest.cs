using System;
using System.IO;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Service.Core;
using Shouldly;
using Xunit;

namespace ChatCommands.Test
{
    public class UptimeCommandTest
    {
        public UptimeCommandTest()
        {
            Sut = new UptimeCommand();

            ChatServiceMock = new Mock<IChatService>();
        }

        public Mock<IChatService> ChatServiceMock { get; set; }

        public UptimeCommand Sut { get; set; }

        public class Execute_Method : UptimeCommandTest
        {
            [Fact]
            public async Task should_not_throw()
            {
                ChatServiceMock.Setup(x => x.GetUptime())
                               .ReturnsAsync(TimeSpan.FromMinutes(97));
                ChatServiceMock.Setup(x => x.SendMessage(It.IsAny<string>()))
                               .ReturnsAsync(true);

                await Sut.Execute(ChatServiceMock.Object, new CommandArgs());
            }

            [Fact]
            public async Task should_format_timespan_properly()
            {
                ChatServiceMock.Setup(x => x.GetUptime())
                               .ReturnsAsync(TimeSpan.FromMinutes(97));
                string chatMessage = null;
                ChatServiceMock.Setup(x => x.SendMessage(It.IsAny<string>()))
                               .ReturnsAsync(true)
                               .Callback<string>(msg => chatMessage = msg);

                await Sut.Execute(ChatServiceMock.Object, new CommandArgs());

                chatMessage.ShouldContain("01:37:00");
            }

            [Fact]
            public async Task should_format_negative_timespan_properly()
            {
                ChatServiceMock.Setup(x => x.GetUptime())
                               .ReturnsAsync(TimeSpan.FromMinutes(-97));
                string chatMessage = null;
                ChatServiceMock.Setup(x => x.SendMessage(It.IsAny<string>()))
                               .ReturnsAsync(true)
                               .Callback<string>(msg => chatMessage = msg);

                await Sut.Execute(ChatServiceMock.Object, new CommandArgs());

                chatMessage.ShouldContain("01:37:00");
            }
        }
    }
}