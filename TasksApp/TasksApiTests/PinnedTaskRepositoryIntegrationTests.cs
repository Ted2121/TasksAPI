using AutoMapper;
using Microsoft.Extensions.Configuration;
using TasksAPI.AutoMapperProfiles;
using TasksAPI.Data;
using TasksAPI.Models;

namespace TasksApiTests;

[TestFixture]
public class PinnedTaskRepositoryIntegrationTests
{
    private PinnedTask _pinnedTask;
    private IPinnedTaskRepository _pinnedTaskRepository;
    private IConfiguration _configuration;

    public PinnedTaskRepositoryIntegrationTests()
    {
        InitializeConfigurationBuilder();
    }

    private void InitializeConfigurationBuilder()
    {
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<PinnedTaskRepositoryIntegrationTests>();
        _configuration = builder.Build();
    }
    

    [SetUp]
    public void SetUp()
    {
        CreatePinnedTask();
        InitializePinnedTaskRepository();
    }
    private void CreatePinnedTask()
    {
        _pinnedTask = new PinnedTask()
        {
            Text = "TestText",
            LabelName = "TestLabel",
            UserId = Guid.NewGuid().ToString()
        };
    }
    private void InitializePinnedTaskRepository()
    {
        _pinnedTaskRepository = new PinnedTaskRepository(_configuration["Tasks: LocalConnectionString"]);
    }

    [TearDown]
    public async Task TearDown() => await _pinnedTaskRepository.DeletePinnedTaskAsync(_pinnedTask.Id);

    [Test]
    public async Task TestingInsertionExpectingPositiveResultAsync()
    {
        // Arrange is done in Set up

        // Act
        _pinnedTask.Id = await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask);

        // Assert
        Assert.That(_pinnedTask.Id, Is.GreaterThan(0));
    }

    [Test]
    public void TestingInsertionThrowsExceptionOnNullUserIdAsync()
    {
        // Arrange 
        _pinnedTask.UserId = null;

        // Act & Assert
        Assert.That(async () => await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask), Throws.Exception);
    }

    [Test]
    public async Task TestingGetPinnedTaskByIdReturnsTheCorrectPinnedTask()
    {
        // Arrange
        _pinnedTask.Id = await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask);

        // Act
        var returnedPinnedTask = await _pinnedTaskRepository.GetPinnedTaskByIdAsync(_pinnedTask.Id);

        // Assert
        Assert.That(returnedPinnedTask, Is.Not.Null);
    }

    [Test]
    public async Task TestingUpdateExpectingPropertiesChangeInDb()
    {
        // Arrange
        _pinnedTask.Id = await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask);
        var newLabel = "NewTestLabel";
        _pinnedTask.LabelName = newLabel;

        // Act
        await _pinnedTaskRepository.UpdatePinnedTaskAsync(_pinnedTask);
        var pinnedTaskInDb = await _pinnedTaskRepository.GetPinnedTaskByIdAsync(_pinnedTask.Id);
        var labelNameFromDb = pinnedTaskInDb.LabelName;

        // Assert
        Assert.That(labelNameFromDb, Is.EqualTo(newLabel));
    }

    [Test]
    public async Task TestingDeletePinnedTaskExpectingSuccess()
    {
        // Arrange
        _pinnedTask.Id = await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask);

        // Act
        var isDeleted = await _pinnedTaskRepository.DeletePinnedTaskAsync(_pinnedTask.Id);

        // Assert
        Assert.That(isDeleted, Is.True);
    }

    [Test]
    public async Task TestingGetAllPinnedTasksExpectingAnyReturned()
    {
        // Arrange
        _pinnedTask.Id = await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask);

        // Act
        var returnedPinnedTasks = await _pinnedTaskRepository.GetAllPinnedTasksAsync();

        // Assert
        Assert.That(returnedPinnedTasks.Any(), Is.True);
    }
}
