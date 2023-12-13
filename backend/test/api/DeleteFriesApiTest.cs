using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using Dapper;
using NUnit.Framework;
using FluentAssertions;
using test.Models;

namespace test.api;

[TestFixture]
public class DeleteFriesApiTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        Helper.TriggerRebuild();
    }

    [Test]
    public async Task FriesCanSuccessfullyBeDeleted()
    {
        using var httpClient = new HttpClient();
        int id = await CreateFries(httpClient);
            
        var deleteResponse = await httpClient.DeleteAsync($"{Helper.ApiBaseUrl}/fries/{id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        using (var conn = Helper.DataSource.OpenConnection())
        {
            var friesInDb = conn.QueryFirstOrDefault<Fries>("SELECT * FROM fries WHERE id = @id", new { id });
            friesInDb.Should().BeNull();
        }
        Console.WriteLine("'Delete Fries API Test' completed successfully.");
    }

    private static async Task<int> CreateFries(HttpClient httpClient)
    {
        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent("Delete Test Fries"), "name");
        formData.Add(new StringContent("35"), "price");

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