using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using NUnit.Framework;
using Dapper;

namespace ApiTests.FriesApiTests;

[TestFixture]
public class CreateFriesApiTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        Helper.TriggerRebuild();
    }

    [TestCase("Create Test Fries", 35)]
    public async Task FriesCanSuccessfullyBeCreated(string name, decimal price)
    {
        using var httpClient = new HttpClient();
        using var formData = new MultipartFormDataContent();

        formData.Add(new StringContent(name), "name");
        formData.Add(new StringContent(price.ToString()), "price"); 

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../test_image.jpg");
        var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        formData.Add(imageContent, "image", "test_image.jpg");

        var httpResponse = await httpClient.PostAsync(Helper.ApiBaseUrl + "/fries", formData);
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdFries = await httpResponse.Content.ReadFromJsonAsync<Fries>();
        createdFries.Should().NotBeNull();
        createdFries.name.Should().Be(name);
        createdFries.price.Should().Be(price);

        using (var conn = Helper.DataSource.OpenConnection())
        {
            var friesInDb = conn.QueryFirstOrDefault<Fries>("SELECT * FROM fries WHERE name = @name", new { name });
            friesInDb.Should().NotBeNull();
            friesInDb.name.Should().Be(name);
            friesInDb.price.Should().Be(price);
        }
        Console.WriteLine("'Create Fries API Test' completed successfully.");
    }
}