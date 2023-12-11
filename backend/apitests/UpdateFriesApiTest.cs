using System.Net.Http.Headers;
using System.Net.Http.Json;
using NUnit.Framework;
using FluentAssertions;

namespace ApiTests;

[TestFixture]
public class UpdateFriesApiTest
{
    [Test]
    public async Task FriesCanSuccessfullyBeUpdated()
    {
        using var httpClient = new HttpClient();

        int id = await CreateFries(httpClient);

        using var updateFormData = new MultipartFormDataContent();
        updateFormData.Add(new StringContent(id.ToString()), "id"); 
        updateFormData.Add(new StringContent("Updated Test Fries"), "name");
        updateFormData.Add(new StringContent("35"), "price");

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../test_image.jpg");
        var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        updateFormData.Add(imageContent, "image", "test_image.jpg");

        var updateResponse = await httpClient.PutAsync($"{Helper.ApiBaseUrl}/fries/{id}", updateFormData);
        updateResponse.EnsureSuccessStatusCode();

        var getResponse = await httpClient.GetAsync($"{Helper.ApiBaseUrl}/fries/{id}");
        getResponse.EnsureSuccessStatusCode();

        var retrievedFries = await getResponse.Content.ReadFromJsonAsync<Fries>();
        retrievedFries.name.Should().Be("Updated Test Fries");
        retrievedFries.price.Should().Be(35);
        
        Console.WriteLine("'Update Fries API Test' completed successfully.");
    }

    private static async Task<int> CreateFries(HttpClient httpClient)
    {
        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent("Test Fries"), "name");
        formData.Add(new StringContent("30"), "price");

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../test_image.jpg");
        var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        formData.Add(imageContent, "image", "test_image.jpg");

        var createResponse = await httpClient.PostAsync($"{Helper.ApiBaseUrl}/fries", formData);
        createResponse.EnsureSuccessStatusCode();

        var createdFries = await createResponse.Content.ReadFromJsonAsync<Burger>();
        return createdFries.id;
    }
}
