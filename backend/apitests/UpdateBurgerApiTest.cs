using System.Net.Http.Headers;
using System.Net.Http.Json;
using NUnit.Framework;
using FluentAssertions;

namespace ApiTests;

[TestFixture]
public class UpdateBurgerApiTest
{
    [Test]
    public async Task BurgerCanSuccessfullyBeUpdated()
    {
        using var httpClient = new HttpClient();

        int id = await CreateBurger(httpClient);

        using var updateFormData = new MultipartFormDataContent();
        updateFormData.Add(new StringContent(id.ToString()), "id");
        updateFormData.Add(new StringContent("Updated Test Burger"), "name");
        updateFormData.Add(new StringContent("95"), "price");
        updateFormData.Add(new StringContent("Updated test burger description"), "description");

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../test_image.jpg");
        var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        updateFormData.Add(imageContent, "image", "test_image.jpg");

        var updateResponse = await httpClient.PutAsync($"{Helper.ApiBaseUrl}/burger/{id}", updateFormData);
        updateResponse.EnsureSuccessStatusCode();

        var getResponse = await httpClient.GetAsync($"{Helper.ApiBaseUrl}/burger/{id}");
        getResponse.EnsureSuccessStatusCode();

        var retrievedBurger = await getResponse.Content.ReadFromJsonAsync<Burger>();
        retrievedBurger.name.Should().Be("Updated Test Burger");
        retrievedBurger.price.Should().Be(95);
        retrievedBurger.description.Should().Be("Updated test burger description");
        
        Console.WriteLine("'Update Burger API Test' completed successfully.");
    }

    private static async Task<int> CreateBurger(HttpClient httpClient)
    {
        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent("Test Burger"), "name");
        formData.Add(new StringContent("90"), "price");
        formData.Add(new StringContent("A test burger description"), "description");

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../test_image.jpg");
        var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        formData.Add(imageContent, "image", "test_image.jpg");

        var createResponse = await httpClient.PostAsync($"{Helper.ApiBaseUrl}/burger", formData);
        createResponse.EnsureSuccessStatusCode();

        var createdBurger = await createResponse.Content.ReadFromJsonAsync<Burger>();
        return createdBurger.id;
    }
}
