using System;
using System.Threading.Tasks;
using Moq;
using Service.Core;
using Shouldly;
using Xunit;

namespace ChatCommands.Test
{
    public class IdeaCommandTest
    {
        public IdeaCommandTest()
        {
            ChatServiceMock = new Mock<IChatService>();
        }

        public Mock<IChatService> ChatServiceMock { get; set; }

        public class Execute_Method : IdeaCommandTest
        {
            [Fact]
            public async Task should_not_throw()
            {
                var args = new CommandArgs { Text = "Just testing!".AsMemory() };

                var sut = new IdeaCommand((x, y) => { });

                await sut.Execute(ChatServiceMock.Object, args);
            }

            [Fact]
            public async Task should_generate_file_path_with_date()
            {
                var args = new CommandArgs { Text = "Just testing!".AsMemory() };

                string generatedIdeaPath = null;
                void fakeWriteIdea(string ideaPath, CommandArgs cargs)
                {
                    generatedIdeaPath = ideaPath;
                }

                var expectedIdeaPath = $@"c:\dev\ideas-{DateTime.Now:MM-dd-yyyy}.txt";

                var sut = new IdeaCommand(fakeWriteIdea);

                await sut.Execute(ChatServiceMock.Object, args);

                generatedIdeaPath.ShouldBe(expectedIdeaPath);
            }

            [Fact]
            public async Task should_invoke_writer()
            {
                var args = new CommandArgs { Text = "Just testing!".AsMemory() };

                var writerInvoked = false;
                void fakeWriteIdea(string ideaPath, CommandArgs cargs)
                {
                    writerInvoked = true;
                }

                var sut = new IdeaCommand(fakeWriteIdea);

                await sut.Execute(ChatServiceMock.Object, args);

                writerInvoked.ShouldBe(true);
            }
        }
    }
}