using AutoMapper;
using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.Models;
using CommandAPI.Profiles;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests
    {
        [Fact]
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //we mock our repo and we use our own AutoMapper instance
            //Arrange
            //mock instance of our repository, only need to pass interface definition
            var mockRepo = new Mock<ICommandRepository>();
            //specify that GetAllCommands will return GetCommands(0)
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));

            var realProfile = new CommandsProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            //we create a concrete instance of IMapper and give it our MapperConfiguration
            IMapper mapper = new Mapper(configuration);
            //we use Object to pass a mock object instance of IcommandRepository
            var controller = new CommandsController(mockRepo.Object, mapper);
        }
        
        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
    }
}
