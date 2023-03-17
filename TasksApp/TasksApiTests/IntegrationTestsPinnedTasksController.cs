//using AutoMapper;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using TasksAPI.AutoMapperProfiles;
//using TasksAPI.Controllers;
//using TasksAPI.Data;
//using TasksAPI.DTOs;
//using TasksAPI.Models;

//namespace TasksApiTests;
//public class IntegrationTestsPinnedTasksController
//{
//    private IPinnedTaskRepository _pinnedTaskRepository;
//    private PinnedTasksController _pinnedTasksController;

//    private IConfiguration _configuration;
//    private IMapper _mapper;

//    private PinnedTaskDto _pinnedTaskDto;
//    private PinnedTask _pinnedTask;
//    private int _pinnedTaskId;

//    public IntegrationTestsPinnedTasksController()
//    {
//        var builder = new ConfigurationBuilder()
//            .AddUserSecrets<IntegrationTestsPinnedTasksController>();
//        _configuration = builder.Build();
//    }

//    private void CreatePinnedTask()
//    {
//        _pinnedTask = new PinnedTask()
//        {
//            Text = "testTextPinnedTask",
//            LabelName = "testLabelPinnedTask",
//            UserId = Guid.NewGuid().ToString(),
//        };
//    }
//    private void CreatePinnedTaskDto()
//    {
//        _pinnedTaskDto = new PinnedTaskDto()
//        {
//            Text = "testTextDto",
//            LabelName = "testLabelDto",
//            UserId = Guid.NewGuid().ToString(),
//        };
//    }
//    private void ConfigureMapper()
//    {
//        var config = new MapperConfiguration(cfg => cfg.AddProfile<PinnedTaskProfile>());
//        _mapper = config.CreateMapper();
//    }
//    private void InitializeRepository()
//    {
//        _pinnedTaskRepository = new PinnedTaskRepository(_configuration["Tasks: LocalConnectionString"]);
//    }
//    private void InitializeController()
//    {
//        _pinnedTasksController = new PinnedTasksController(_pinnedTaskRepository, _mapper);
//    }

//    [SetUp]
//    public void SetUp()
//    {
//        ConfigureMapper();
//        CreatePinnedTaskDto();
//        CreatePinnedTask();
//        InitializeRepository();
//        InitializeController();
//    }

//    [TearDown]
//    public async Task TearDown()
//    {
//        await _pinnedTaskRepository.DeletePinnedTaskAsync(_pinnedTaskId);
//    }

//    [Test]
//    public async Task TestingGetByIdExpectingPinnedTaskReturned()
//    {
//        // Arrange in SetUp
//        _pinnedTaskId = await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask);

//        // Act
//        var pinnedTaskDtoReturned = await _pinnedTasksController.GetPinnedTaskByIdAsync(_pinnedTaskId);

//        // Assert
//        Assert.That(pinnedTaskDtoReturned, Is.Not.Null);
//    }

//    [Test]
//    public async Task TestingGetByIdWithIdZeroExpectingException()
//    {
//        // Arrange in setup

//        // Act
//        var pinnedTaskDto = await _pinnedTasksController.GetPinnedTaskByIdAsync(0);

//        // Assert 
//        Assert.That(pinnedTaskDto.Result, Is.InstanceOf<NotFoundResult>());
//    }

//    [Test]
//    public async Task TestingGetAllPinnedTasksExpectingAnyReturned()
//    {
//        // Arrange
//        _pinnedTaskId = await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask);

//        // Act
//        var result = await _pinnedTasksController.GetAllPinnedTasksAsync();

//        var pinnedTaskDtos = result.Value;

//        // Assert
//        Assert.That(pinnedTaskDtos?.Any(), Is.True);
//    }
//}
