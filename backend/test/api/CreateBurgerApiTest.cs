using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using NUnit.Framework;
using test.Models;

namespace test.api;

[TestFixture]
    public class CreateBurgerApiTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Helper.TriggerRebuild();
        }

        [TestCase("Create Test Burger", 95, "Create test burger description")]
        public async Task BurgerCanSuccessfullyBeCreatedFromHttpRequest(string name, decimal price, string description)
        {
            using var httpClient = new HttpClient();
            using var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(name), "name");
            formData.Add(new StringContent(price.ToString()), "price"); 
            formData.Add(new StringContent(description), "description"); 

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../test_image.jpg");
            var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            formData.Add(imageContent, "image", "test_image.jpg");
            
            var token = await Helper.GetAuthenticationToken("Sohaib", "burger");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await httpClient.PostAsync(Helper.ApiBaseUrl + "/burger", formData);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var createdBurger = await httpResponse.Content.ReadFromJsonAsync<Burger>();
            createdBurger.Should().NotBeNull();
            createdBurger.name.Should().Be(name);
            createdBurger.price.Should().Be(price);
            createdBurger.description.Should().Be(description);

            using (var conn = Helper.DataSource.OpenConnection())
            {
                var burgerInDb = conn.QueryFirstOrDefault<Burger>("SELECT * FROM burgers WHERE name = @name", new { name });
                burgerInDb.Should().NotBeNull();
                burgerInDb.name.Should().Be(name);
                burgerInDb.price.Should().Be(price);
                burgerInDb.description.Should().Be(description);
            }
            Console.WriteLine("'Create Burger API Test' completed successfully.");
        }
    }
