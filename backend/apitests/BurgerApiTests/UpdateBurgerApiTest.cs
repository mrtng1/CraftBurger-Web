using System.Net.Http.Headers;
using System.Net.Http.Json;
using Dapper;
using NUnit.Framework;
using FluentAssertions;

namespace ApiTests.BurgerApiTests;

[TestFixture]
public class UpdateBurgerApiTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        Helper.TriggerRebuild();
    }

    [Test]
    public async Task BurgerCanSuccessfullyBeUpdated()
    {
        using var httpClient = new HttpClient();
        int id = await CreateBurger(httpClient, "Test Burger", 90, "Test burger description");

        using var updateFormData = new MultipartFormDataContent();
        updateFormData.Add(new StringContent(id.ToString()), "id");
        updateFormData.Add(new StringContent("Update Test Burger"), "name");
        updateFormData.Add(new StringContent("95"), "price");
        updateFormData.Add(new StringContent("Update test burger description"), "description");

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../test_image.jpg");
        var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        updateFormData.Add(imageContent, "image", "test_image.jpg");

        var updateResponse = await httpClient.PutAsync($"{Helper.ApiBaseUrl}/burger/{id}", updateFormData);
        updateResponse.EnsureSuccessStatusCode();

        using (var conn = Helper.DataSource.OpenConnection())
        {
            var updatedBurger = conn.QueryFirstOrDefault<Burger>("SELECT * FROM burgers WHERE id = @Id", new { Id = id });
            updatedBurger.Should().NotBeNull();
            updatedBurger.name.Should().Be("Update Test Burger");
            updatedBurger.price.Should().Be(95);
            updatedBurger.description.Should().Be("Update test burger description");
        }
        Console.WriteLine("'Update Burger API Test' completed successfully.");
    }

    private static async Task<int> CreateBurger(HttpClient httpClient, string name, decimal price, string description)
    {
        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(name), "name");
        formData.Add(new StringContent(price.ToString()), "price");
        formData.Add(new StringContent(description), "description");

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
