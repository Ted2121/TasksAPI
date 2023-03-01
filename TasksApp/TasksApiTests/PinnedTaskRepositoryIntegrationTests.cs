using Microsoft.Extensions.Configuration;
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
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<PinnedTaskRepositoryIntegrationTests>();
        _configuration = builder.Build();
    }


    [SetUp]
    public void SetUp()
    {
        _pinnedTask = new PinnedTask()
        {
            Text = "TestText",
            LabelName = "TestLabel",
            UserId = Guid.NewGuid().ToString()
        };

        _pinnedTaskRepository = new PinnedTaskRepository(_configuration["Tasks: LocalConnectionString"]);
    }

    // Teardown not possible because of identity generated id

    [Test]
    public async Task TestingInsertionExpectingPositiveResultAsync()
    {
        // Arrange is done in Set up

        // Act
        _pinnedTask.Id = await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask);

        // Assert
        try
        {
            Assert.That(_pinnedTask.Id, Is.GreaterThan(0));
        }
        finally
        {
            await _pinnedTaskRepository.DeletePinnedTaskAsync(_pinnedTask.Id);
        }
    }

    [Test]
    public async Task TestingInsertionThrowsExceptionOnNullUserIdAsync()
    {
        // Arrange 
        _pinnedTask.UserId = null;

        // Act & Assert
        try
        {
            Assert.That(async () => await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask), Throws.Exception);
        }
        finally
        {
            await _pinnedTaskRepository.DeletePinnedTaskAsync(_pinnedTask.Id);
        }

    }

    [Test]
    public async Task TestingGetPinnedTaskByIdReturnsTheCorrectPinnedTask()
    {
        // Arrange
        _pinnedTask.Id = await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask);

        // Act
        var returnedPinnedTask = await _pinnedTaskRepository.GetPinnedTaskByIdAsync(_pinnedTask.Id);

        // Assert
        try
        {
            Assert.That(returnedPinnedTask, Is.Not.Null);
        }
        finally
        {
            await _pinnedTaskRepository.DeletePinnedTaskAsync(_pinnedTask.Id);
        }
    }

    [Test]
    public async Task TestingUpdateExpectingPropertiesChangeInDb()
    {
        try
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
        finally
        {
            await _pinnedTaskRepository.DeletePinnedTaskAsync(_pinnedTask.Id);
        }
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
        try
        {
        // Arrange
        _pinnedTask.Id = await _pinnedTaskRepository.InsertPinnedTaskAsync(_pinnedTask);

        // Act
        var returnedPinnedTasks = await _pinnedTaskRepository.GetAllPinnedTasksAsync();

        // Assert
        Assert.That(returnedPinnedTasks.Any(), Is.True);

        }
        finally 
        {
            await _pinnedTaskRepository.DeletePinnedTaskAsync(_pinnedTask.Id);
        }
    }
}
