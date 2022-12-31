using AutoMapper;
using Castle.Core.Logging;
using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Profiles;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandRepository> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandRepository>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }
        public void Dispose()
        {
            mockRepo = null;
            realProfile = null;
            configuration = null;
            mapper = null;
        }

        //GetAllCommands tests

        [Fact]
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //we mock our repo and we use our own AutoMapper instance
            //we mock repo to return 0 resources
            //Arrange
            //mock instance of our repository, only need to pass interface definition
            //specify that GetAllCommands will return GetCommands(0)
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));
            //we create a concrete instance of IMapper and give it our MapperConfiguration
            //we use Object to pass a mock object instance of IcommandRepository
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            //check if it's 200 OK
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandItems_ReturnsSingleItem_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.GetAllCommands();
            //Assert
            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<CommandReadDto>;
            //assert we have a single command in out commands List
            Assert.Single(commands);
        }

        [Fact]
        public void GetCommandItems_Returns200Ok_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.GetAllCommands();
            //Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandItems_ReturnsCorrectObjectType_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.GetAllCommands();
            //Assert
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
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

        //GetCommandById tests

        [Fact]
        public void GetCommandByID_Returns404NotFound_WhenNonExistentIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.GetCommandById(1);
            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommandByID_Returns200Ok_WhenValidIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(new Command
            {
                Id = 1,
                HowTo = "mock",
                Platform = "mock",
                CommandLine = "mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.GetCommandById(0);
            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandByID_ReturnsCorrectObjectType_WhenValidIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(new Command
            {
                Id = 1,
                HowTo = "mock",
                Platform = "mock",
                CommandLine = "mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.GetCommandById(0);
            //Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        //CreateCommand tests

        [Fact]
        public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
            //Arrange
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.CreateCommand(new CommandCreateDto { });
            //Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
        {
            //Arrange
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.CreateCommand(new CommandCreateDto { });
            //Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        //EditCommand tests
        [Fact]
        public void EditCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(new Command
            { 
                HowTo = "mock",
                Platform = "mock",
                CommandLine = "mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);
            var result = controller.EditCommand(0, new CommandEditDto { });
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void EditCommand_Returns404NotFound_WhenNonExistentResourceSubmitted()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);
            var result = controller.EditCommand(0, new CommandEditDto { });
            Assert.IsType<NotFoundResult>(result);
        }

        //DeleteCommand tests
        [Fact]
        public void DeleteCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(new Command
            {
                HowTo = "mock",
                Platform = "mock",
                CommandLine = "mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);
            var result = controller.DeleteCommand(0);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns404NotFound_WhenNonExistentResourceSubmitted()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);
            var result = controller.DeleteCommand(0);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
