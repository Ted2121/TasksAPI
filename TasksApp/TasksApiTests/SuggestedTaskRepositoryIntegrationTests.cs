using Microsoft.Extensions.Configuration;
using TasksAPI.Data;
using TasksAPI.Models;

namespace TasksApiTests;
public class SuggestedTaskRepositoryIntegrationTests
{
    private SuggestedTask _suggestedTask;
    private SuggestedLabel _suggestedLabel;
    private ISuggestedTaskRepository _suggestedTaskRepository;
    private ISuggestedLabelRepository _suggestedLabelRepository;
    private IConfiguration _configuration;

    public SuggestedTaskRepositoryIntegrationTests()
    {
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<SuggestedTaskRepositoryIntegrationTests>();
        _configuration = builder.Build();
    }

    [SetUp]
    public async Task SetUp()
    {
        _suggestedTaskRepository = new SuggestedTaskRepository(_configuration["Tasks: LocalConnectionString"]);

        _suggestedLabelRepository = new SuggestedLabelRepository(_configuration["Tasks: LocalConnectionString"]);

        _suggestedLabel = new SuggestedLabel()
        {
            Name = "TestLabel"
        };

        _suggestedLabel.Id = await _suggestedLabelRepository.InsertSuggestedLabelAsync(_suggestedLabel);



        _suggestedTask = new SuggestedTask()
        {
            Text = "TestText",
            FKSuggestedLabelId = _suggestedLabel.Id,
        };


    }

    [TearDown]
    public async Task TearDown()
    {
        await _suggestedTaskRepository.DeleteSuggestedTaskAsync(_suggestedTask.Id);
        await _suggestedLabelRepository.DeleteSuggestedLabelAsync(_suggestedLabel.Id);
    }

    [Test]
    public async Task TestingInsertionExpectingPositiveResultAsync()
    {
        // Arrange is done in Set up

        // Act
        _suggestedTask.Id = await _suggestedTaskRepository.InsertSuggestedTaskAsync(_suggestedTask);

        // Assert
        Assert.That(_suggestedTask.Id, Is.GreaterThan(0));
    }

    [Test]
    public void TestingInsertionThrowsExceptionOnNullUserIdAsync()
    {
        // Arrange 
        _suggestedTask.Text = null;

        // Act & Assert
        Assert.That(async () => await _suggestedTaskRepository.InsertSuggestedTaskAsync(_suggestedTask), Throws.Exception);

    }

    [Test]
    public async Task TestingGetSuggestedTaskByIdReturnsTheCorrectSuggestedTask()
    {
        // Arrange
        _suggestedTask.Id = await _suggestedTaskRepository.InsertSuggestedTaskAsync(_suggestedTask);

        // Act
        var returnedSuggestedTask = await _suggestedTaskRepository.GetSuggestedTaskByIdAsync(_suggestedTask.Id);

        // Assert
        Assert.That(returnedSuggestedTask, Is.Not.Null);
    }

    [Test]
    public async Task TestingUpdateExpectingPropertiesChangeInDb()
    {
        // Arrange
        // Insert as created in set up
        _suggestedTask.Id = await _suggestedTaskRepository.InsertSuggestedTaskAsync(_suggestedTask);

        // Change the Text to a new Text
        var newSuggestedTask = "NewTestSuggestedTask";
        _suggestedTask.Text = newSuggestedTask;

        // Act
        // Update and get the Text value currently in DB
        await _suggestedTaskRepository.UpdateSuggestedTaskAsync(_suggestedTask);
        var suggestedTaskInDb = await _suggestedTaskRepository.GetSuggestedTaskByIdAsync(_suggestedTask.Id);
        var suggestedTaskNameFromDb = suggestedTaskInDb.Text;

        // Assert
        // Check if the Text in DB is equal to the intended change
        Assert.That(suggestedTaskNameFromDb, Is.EqualTo(newSuggestedTask));
    }

    [Test]
    public async Task TestingDeleteSuggestedTaskExpectingSuccess()
    {
        // Arrange
        _suggestedTask.Id = await _suggestedTaskRepository.InsertSuggestedTaskAsync(_suggestedTask);

        // Act
        var isDeleted = await _suggestedTaskRepository.DeleteSuggestedTaskAsync(_suggestedTask.Id);

        // Assert
        Assert.That(isDeleted, Is.True);
    }

    [Test]
    public async Task TestingGetAllSuggestedTasksExpectingAnyReturned()
    {
        // Arrange
        _suggestedTask.Id = await _suggestedTaskRepository.InsertSuggestedTaskAsync(_suggestedTask);

        // Act
        var returnedSuggestedTasks = await _suggestedTaskRepository.GetAllSuggestedTasksAsync();

        // Assert
        Assert.That(returnedSuggestedTasks.Any(), Is.True);
    }
}
