using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using Dapper;
using NUnit.Framework;
using FluentAssertions;
using test.Models;

namespace test.api;

[TestFixture]
    public class DeleteBurgerApiTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Helper.TriggerRebuild();
        }

        [Test]
        public async Task BurgerCanSuccessfullyBeDeleted()
        {
            using var httpClient = new HttpClient();
            int id = await CreateBurger(httpClient);

            var deleteResponse = await httpClient.DeleteAsync($"{Helper.ApiBaseUrl}/burger/{id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await httpClient.GetAsync($"{Helper.ApiBaseUrl}/burger/{id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using (var conn = Helper.DataSource.OpenConnection())
            {
                var burgerInDb = conn.QueryFirstOrDefault<Burger>("SELECT * FROM burgers WHERE id = @id", new { id });
                burgerInDb.Should().BeNull();
            }
            Console.WriteLine("'Delete Burger API Test' completed successfully.");
        }

        private static async Task<int> CreateBurger(HttpClient httpClient)
        {
            using var formData = new MultipartFormDataContent();
            formData.Add(new StringContent("Delete Test Burger"), "name");
            formData.Add(new StringContent("95"), "price");
            formData.Add(new StringContent("Delete test burger description"), "description");

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