using CommandAPI.Models;
using System;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable //used for code cleanup
    {
        Command testCommand;

        public CommandTests()
        {
            testCommand = new Command
            {
                HowTo = "Do something",
                Platform = "Some platform",
                CommandLine = "Some commandline"
            };
        }
        //it will clean changes after each test, to each test will be passed testCommand
        public void Dispose()
        {
            testCommand = null;
        }

        [Fact]
        public void CanChangeHowTo()
        {
            //Arrange
            
            //Act
            testCommand.HowTo = "Execute something";

            //Assert
            Assert.Equal("Execute something", testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlatform()
        {
            //Arrange

            //Act
            testCommand.Platform = "Best platform";

            //Assert
            Assert.Equal("Best platform", testCommand.Platform);
            Assert.Equal("Do something", testCommand.HowTo);
        }

        [Fact]
        public void CanChangeCommand()
        {
            //Arrange

            //Act
            testCommand.CommandLine = "Best commandline";

            //Assert
            Assert.Equal("Best commandline", testCommand.CommandLine);
        }



    }
}
