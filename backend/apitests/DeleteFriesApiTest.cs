using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using NUnit.Framework;
using FluentAssertions;

namespace apitests;

[TestFixture]
public class DeleteFriessApiTest
{
    [Test]
    public async Task BurgerCanSuccessfullyBeDeleted()
    {
        using var httpClient = new HttpClient();

        int id = await CreateFries(httpClient);

        var deleteResponse = await httpClient.DeleteAsync($"{Helper.ApiBaseUrl}/fries/{id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await httpClient.GetAsync($"{Helper.ApiBaseUrl}/burger/{id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<int> CreateFries(HttpClient httpClient)
    {
        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent("Test Fries"), "name");
        formData.Add(new StringContent("36"), "price");

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../test_image.jpg");
        var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        formData.Add(imageContent, "image", "test_image.jpg");

        var createResponse = await httpClient.PostAsync($"{Helper.ApiBaseUrl}/fries", formData);
        createResponse.EnsureSuccessStatusCode();

        var createdFries = await createResponse.Content.ReadFromJsonAsync<Fries>();
        return createdFries.id;
    }
}