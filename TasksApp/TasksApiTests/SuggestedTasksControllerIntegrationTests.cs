using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using TasksAPI;
using TasksAPI.DTOs;

namespace TasksApiTests;

public class SuggestedTasksControllerIntegrationTests
{
    private SuggestedTaskDto _suggestedTaskDto;
    private SuggestedLabelDto _suggestedLabelDto;

    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _httpClient;

    private string _baseUrl = "api/v1/SuggestedTasks";

    private void InitializeWebApplication()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
    }

    private void InitializeHttpClient()
    {
        _httpClient = _webApplicationFactory.CreateDefaultClient();
    }

    private void InitializeSuggestedTaskDto()
    {
        _suggestedTaskDto = new SuggestedTaskDto()
        {
            Text = "Test",
            FKSuggestedLabelId = _suggestedLabelDto.Id
        };
    }

    private void InitializeSuggestedLabelDto()
    {
        _suggestedLabelDto = new SuggestedLabelDto()
        {
            Name = "Test"
        };
    }

    private async Task InsertSuggestedLabel()
    {
        int returnedId;

        var response = await _httpClient.PostAsJsonAsync("api/v1/SuggestedLabels", _suggestedLabelDto);

        Int32.TryParse(await response.Content.ReadAsStringAsync(), out returnedId);

        _suggestedLabelDto.Id = returnedId;
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        InitializeWebApplication();
        InitializeHttpClient();
    }

    [SetUp]
    public async Task SetUpAsync()
    {
        InitializeSuggestedLabelDto();
        await InsertSuggestedLabel();
        InitializeSuggestedTaskDto();
    }

    [TearDown]
    public async Task TearDown() => _httpClient.DeleteAsync($"{_baseUrl}{_suggestedTaskDto.Id}");

    [Test]
    public async Task ShouldReturnIdOfInsertedSuggestedTask()
    {
        // Arrange
        int returnedId;

        // Act
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, _suggestedTaskDto);
        Int32.TryParse(await response.Content.ReadAsStringAsync(), out returnedId);

        _suggestedTaskDto.Id = returnedId;


        // Assert
        Assert.That(returnedId, Is.GreaterThan(0));
    }

    [Test]
    public async Task ShouldReturnAnySuggestedTaskWhenGettingAll()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _suggestedTaskDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _suggestedTaskDto.Id = returnedId;

        // Act
        var getResponse = await _httpClient.GetAsync(_baseUrl);
        var data = await getResponse.Content.ReadAsStringAsync();

        List<SuggestedTaskDto> suggestedTasks = JsonConvert.DeserializeObject<List<SuggestedTaskDto>>(data);

        // Assert
        Assert.That(suggestedTasks.Any, Is.True);
    }

    [Test]
    public async Task ShouldReturnNoContentAfterDeletingSuggestedTask()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _suggestedTaskDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _suggestedTaskDto.Id = returnedId;

        // Act
        var deleteResponse = await _httpClient.DeleteAsync($"{_baseUrl}/{_suggestedTaskDto.Id}");
        var statusCode = deleteResponse.StatusCode;

        // Assert
        Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task ShouldReturnNoContentWhenUpdatingSuggestedTask()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _suggestedTaskDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _suggestedTaskDto.Id = returnedId;

        // Act
        _suggestedTaskDto.Text = "UpdatedName";

        var updateResponse = await _httpClient.PutAsJsonAsync($"{_baseUrl}/updatesuggestedtask/{_suggestedTaskDto.Id}", _suggestedTaskDto);

        var statusCode = updateResponse.StatusCode;

        // Assert
        Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task ShouldReturnSuggestedTaskWhenGettingById()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _suggestedTaskDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _suggestedTaskDto.Id = returnedId;

        // Act
        var getResponse = await _httpClient.GetAsync($"{_baseUrl}/{_suggestedTaskDto.Id}");
        var data = await getResponse.Content.ReadAsStringAsync();
        var suggestedTaskDto = JsonConvert.DeserializeObject<SuggestedTaskDto>(data);

        // Assert
        Assert.That(suggestedTaskDto, Is.Not.Null);
    }
}
