using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using NUnit.Framework;
using FluentAssertions;
using apitests;

namespace ApiTests
{
    [TestFixture]
    public class DeleteBurgerApiTest
    {
        [Test]
        public async Task BurgerCanSuccessfullyBeDeleted()
        {
            using var httpClient = new HttpClient();

            // Step 1: Create a burger
            int burgerId = await CreateBurger(httpClient);

            // Step 2: Delete the burger
            var deleteResponse = await httpClient.DeleteAsync($"{Helper.ApiBaseUrl}/burger/{burgerId}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Step 3: Verify deletion
            var getResponse = await httpClient.GetAsync($"{Helper.ApiBaseUrl}/burger/{burgerId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<int> CreateBurger(HttpClient httpClient)
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
}