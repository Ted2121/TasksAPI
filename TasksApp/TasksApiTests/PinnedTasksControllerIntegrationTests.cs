using Microsoft.AspNetCore.Mvc.Testing;
using TasksAPI.DTOs;
using System.Net.Http.Json;
using TasksAPI;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Serilog.Sinks.InMemory;
using Newtonsoft.Json;
using System.Net;

namespace TasksApiTests;

[TestFixture]
public class PinnedTasksControllerIntegrationTests
{
    private PinnedTaskDto _pinnedTaskDto;

    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _httpClient;

    private string _baseUrl = "api/v1/PinnedTasks";

    private void InitializeWebApplication()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
    }

    private void InitializeHttpClient()
    {
        _httpClient = _webApplicationFactory.CreateDefaultClient();
    }

    private void InitializePinnedTaskDto()
    {
        _pinnedTaskDto = new PinnedTaskDto()
        {
            Text = "Test",
            LabelName = "TestLabel",
            UserId = Guid.NewGuid().ToString()
        };
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        InitializeWebApplication();
        InitializeHttpClient();
    }

    [SetUp]
    public void SetUp()
    {
        InitializePinnedTaskDto();
    }

    [TearDown]
    public async Task TearDown() => _httpClient.DeleteAsync($"{_baseUrl}{_pinnedTaskDto.Id}");

    [Test]
    public async Task ShouldReturnIdOfInsertedPinnedTask()
    {
        // Arrange
        int returnedId;

        // Act
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, _pinnedTaskDto);
        Int32.TryParse(await response.Content.ReadAsStringAsync(), out returnedId);

        _pinnedTaskDto.Id = returnedId;


        // Assert
        Assert.That(returnedId, Is.GreaterThan(0));
    }

    [Test]
    public async Task ShouldReturnAnyPinnedTaskWhenGettingAll()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _pinnedTaskDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _pinnedTaskDto.Id = returnedId;

        // Act
        var getResponse = await _httpClient.GetAsync(_baseUrl);
        var data = await getResponse.Content.ReadAsStringAsync();

        List<PinnedTaskDto> pinnedTasks = JsonConvert.DeserializeObject<List<PinnedTaskDto>>(data);

        // Assert
        Assert.That(pinnedTasks.Any, Is.True);
    }

    [Test]
    public async Task ShouldReturnNoContentAfterDeletingPinnedTask()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _pinnedTaskDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _pinnedTaskDto.Id = returnedId;

        // Act
        var deleteResponse = await _httpClient.DeleteAsync($"{_baseUrl}{_pinnedTaskDto.Id}");
        var statusCode = deleteResponse.StatusCode;

        // Assert
        Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task ShouldReturnNoContentWhenUpdatingPinnedTask()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _pinnedTaskDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _pinnedTaskDto.Id = returnedId;

        // Act
        _pinnedTaskDto.Text = "UpdatedText";

        var updateResponse = await _httpClient.PutAsJsonAsync($"{_baseUrl}/updatepinnedtask/{_pinnedTaskDto.Id}", _pinnedTaskDto);

        var statusCode = updateResponse.StatusCode; 

        // Assert
        Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task ShouldReturnPinnedTaskWhenGettingById()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _pinnedTaskDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _pinnedTaskDto.Id = returnedId;

        // Act
        var getResponse = await _httpClient.GetAsync($"{_baseUrl}/{_pinnedTaskDto.Id}");
        var data = await getResponse.Content.ReadAsStringAsync();
        var pinnedTaskDto = JsonConvert.DeserializeObject<PinnedTaskDto>(data);

        // Assert
        Assert.That(pinnedTaskDto, Is.Not.Null);
    }
}
