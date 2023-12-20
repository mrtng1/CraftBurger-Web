using System.Net.Http.Headers;
using System.Net.Http.Json;
using Dapper;
using NUnit.Framework;
using FluentAssertions;
using test.Models;

namespace test.api;

[TestFixture]
public class UpdateFriesApiTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        Helper.TriggerRebuild();
    }

    [Test]
    public async Task FriesCanSuccessfullyBeUpdated()
    {
        using var httpClient = new HttpClient();
        int id = await CreateFries(httpClient, "Test Fries", 30);

        using var updateFormData = new MultipartFormDataContent();
        updateFormData.Add(new StringContent(id.ToString()), "id");
        updateFormData.Add(new StringContent("Updated Test Fries"), "name");
        updateFormData.Add(new StringContent("35"), "price");

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../test_image.jpg");
        var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        updateFormData.Add(imageContent, "image", "test_image.jpg");
        
        var token = await Helper.GetAuthenticationToken("Sohaib", "burger");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var updateResponse = await httpClient.PutAsync($"{Helper.ApiBaseUrl}/fries/{id}", updateFormData);
        updateResponse.EnsureSuccessStatusCode();

        using (var conn = Helper.DataSource.OpenConnection())
        {
            var updatedFries = conn.QueryFirstOrDefault<Fries>("SELECT * FROM fries WHERE id = @Id", new { Id = id });
            updatedFries.Should().NotBeNull();
            updatedFries.name.Should().Be("Updated Test Fries");
            updatedFries.price.Should().Be(35);
        }
        Console.WriteLine("'Update Fries API Test' completed successfully.");
    }

    private static async Task<int> CreateFries(HttpClient httpClient, string name, decimal price)
    {
        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(name), "name");
        formData.Add(new StringContent(price.ToString()), "price");

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../test_image.jpg");
        var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        formData.Add(imageContent, "image", "test_image.jpg");
        
        var token = await Helper.GetAuthenticationToken("Sohaib", "burger");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await httpClient.PostAsync($"{Helper.ApiBaseUrl}/fries", formData);
        createResponse.EnsureSuccessStatusCode();

        var createdFries = await createResponse.Content.ReadFromJsonAsync<Fries>();
        return createdFries.id;
    }
}
