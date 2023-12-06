using System.Net.Http.Headers;
using System.Net;
using NUnit.Framework;
using FluentAssertions;
using apitests;
using System.Net.Http.Json;

namespace ApiTests
{
    [TestFixture]
    public class CreateBurgerApiTest
    {
        [TestCase("Veggie", 95, "A veggie burger")]
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

            var httpResponse = await httpClient.PostAsync(Helper.ApiBaseUrl + "/burger", formData);
            
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var createdBurger = await httpResponse.Content.ReadFromJsonAsync<Burger>();
            createdBurger.Should().NotBeNull();
            createdBurger.name.Should().Be(name);
            createdBurger.price.Should().Be(price);
            createdBurger.description.Should().Be(description);
        }
    }
}
