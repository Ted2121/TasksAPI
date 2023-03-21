using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using TasksAPI;
using TasksAPI.DTOs;

namespace TasksApiTests;

public class SuggestedLabelsControllerIntegrationTests
{
    private SuggestedLabelDto _suggestedLabelDto;

    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _httpClient;

    private string _baseUrl = "api/v1/SuggestedLabels";

    private void InitializeWebApplication()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
    }

    private void InitializeHttpClient()
    {
        _httpClient = _webApplicationFactory.CreateDefaultClient();
    }

    private void InitializeSuggestedLabelDto()
    {
        _suggestedLabelDto = new SuggestedLabelDto()
        {
            Name = "Test",
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
        InitializeSuggestedLabelDto();
    }

    [TearDown]
    public async Task TearDown() => _httpClient.DeleteAsync($"{_baseUrl}{_suggestedLabelDto.Id}");

    [Test]
    public async Task ShouldReturnIdOfInsertedSuggestedLabel()
    {
        // Arrange
        int returnedId;

        // Act
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, _suggestedLabelDto);
        Int32.TryParse(await response.Content.ReadAsStringAsync(), out returnedId);

        _suggestedLabelDto.Id = returnedId;


        // Assert
        Assert.That(returnedId, Is.GreaterThan(0));
    }

    [Test]
    public async Task ShouldReturnAnySuggestedLabelWhenGettingAll()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _suggestedLabelDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _suggestedLabelDto.Id = returnedId;

        // Act
        var getResponse = await _httpClient.GetAsync(_baseUrl);
        var data = await getResponse.Content.ReadAsStringAsync();

        List<SuggestedLabelDto> suggestedLabels = JsonConvert.DeserializeObject<List<SuggestedLabelDto>>(data);

        // Assert
        Assert.That(suggestedLabels.Any, Is.True);
    }

    [Test]
    public async Task ShouldReturnNoContentAfterDeletingSuggestedLabel()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _suggestedLabelDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _suggestedLabelDto.Id = returnedId;

        // Act
        var deleteResponse = await _httpClient.DeleteAsync($"{_baseUrl}/{_suggestedLabelDto.Id}");
        var statusCode = deleteResponse.StatusCode;

        // Assert
        Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task ShouldReturnNoContentWhenUpdatingSuggestedLabel()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _suggestedLabelDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _suggestedLabelDto.Id = returnedId;

        // Act
        _suggestedLabelDto.Name = "UpdatedName";

        var updateResponse = await _httpClient.PutAsJsonAsync($"{_baseUrl}/updatesuggestedlabel/{_suggestedLabelDto.Id}", _suggestedLabelDto);

        var statusCode = updateResponse.StatusCode;

        // Assert
        Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task ShouldReturnSuggestedLabelWhenGettingById()
    {
        // Arrange
        int returnedId;

        var postResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _suggestedLabelDto);
        Int32.TryParse(await postResponse.Content.ReadAsStringAsync(), out returnedId);

        _suggestedLabelDto.Id = returnedId;

        // Act
        var getResponse = await _httpClient.GetAsync($"{_baseUrl}/{_suggestedLabelDto.Id}");
        var data = await getResponse.Content.ReadAsStringAsync();
        var suggestedLabelDto = JsonConvert.DeserializeObject<SuggestedLabelDto>(data);

        // Assert
        Assert.That(suggestedLabelDto, Is.Not.Null);
    }
}
