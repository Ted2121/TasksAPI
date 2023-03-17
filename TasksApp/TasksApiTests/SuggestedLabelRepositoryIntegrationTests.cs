using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksAPI.Data;
using TasksAPI.Models;

namespace TasksApiTests;
public class SuggestedLabelRepositoryIntegrationTests
{
    private SuggestedLabel _suggestedLabel;
    private ISuggestedLabelRepository _suggestedLabelRepository;
    private IConfiguration _configuration;

    public SuggestedLabelRepositoryIntegrationTests()
    {
        InitializeConfigurationBuilder();
    }


    [SetUp]
    public void SetUp()
    {
        CreateSuggestedLabel();
        InitializeSuggestedLabelRepository();
    }

    private void InitializeConfigurationBuilder()
    {
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<SuggestedLabelRepositoryIntegrationTests>();
        _configuration = builder.Build();
    }
    private void CreateSuggestedLabel()
    {
        _suggestedLabel = new SuggestedLabel()
        {
            Name = "test name",
        };
    }
    private void InitializeSuggestedLabelRepository()
    {
        _suggestedLabelRepository = new SuggestedLabelRepository(_configuration["Tasks: LocalConnectionString"]);
    }

    [TearDown]
    public async Task TearDown() => await _suggestedLabelRepository.DeleteSuggestedLabelAsync(_suggestedLabel.Id);

    [Test]
    public async Task TestingInsertionExpectingPositiveResultAsync()
    {
        // Arrange is done in Set up

        // Act
        _suggestedLabel.Id = await _suggestedLabelRepository.InsertSuggestedLabelAsync(_suggestedLabel);

        // Assert
        Assert.That(_suggestedLabel.Id, Is.GreaterThan(0));
    }

    [Test]
    public void TestingInsertionThrowsExceptionOnNullUserIdAsync()
    {
        // Arrange 
        _suggestedLabel.Name = null;

        // Act & Assert
        Assert.That(async () => await _suggestedLabelRepository.InsertSuggestedLabelAsync(_suggestedLabel), Throws.Exception);
    }

    [Test]
    public async Task TestingGetSuggestedLabelByIdReturnsTheCorrectSuggestedLabel()
    {
        // Arrange
        _suggestedLabel.Id = await _suggestedLabelRepository.InsertSuggestedLabelAsync(_suggestedLabel);

        // Act
        var returnedSuggestedLabel = await _suggestedLabelRepository.GetSuggestedLabelByIdAsync(_suggestedLabel.Id);

        // Assert
        Assert.That(returnedSuggestedLabel, Is.Not.Null);
    }

    [Test]
    public async Task TestingUpdateExpectingPropertiesChangeInDb()
    {
        // Arrange
        _suggestedLabel.Id = await _suggestedLabelRepository.InsertSuggestedLabelAsync(_suggestedLabel);
        var newName = "NewSuggestedLabelName";
        _suggestedLabel.Name = newName;

        // Act
        await _suggestedLabelRepository.UpdateSuggestedLabelAsync(_suggestedLabel);
        var suggestedLabelInDb = await _suggestedLabelRepository.GetSuggestedLabelByIdAsync(_suggestedLabel.Id);
        var labelNameFromDb = suggestedLabelInDb.Name;

        // Assert
        Assert.That(labelNameFromDb, Is.EqualTo(newName));
    }

    [Test]
    public async Task TestingDeleteSuggestedLabelExpectingSuccess()
    {
        // Arrange
        _suggestedLabel.Id = await _suggestedLabelRepository.InsertSuggestedLabelAsync(_suggestedLabel);

        // Act
        var isDeleted = await _suggestedLabelRepository.DeleteSuggestedLabelAsync(_suggestedLabel.Id);

        // Assert
        Assert.That(isDeleted, Is.True);
    }

    [Test]
    public async Task TestingGetAllSuggestedLabelsExpectingAnyReturned()
    {
        // Arrange
        _suggestedLabel.Id = await _suggestedLabelRepository.InsertSuggestedLabelAsync(_suggestedLabel);

        // Act
        var returnedSuggestedLabels = await _suggestedLabelRepository.GetAllSuggestedLabelsAsync();

        // Assert
        Assert.That(returnedSuggestedLabels.Any(), Is.True);
    }
}
