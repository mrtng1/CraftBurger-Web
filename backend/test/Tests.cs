using backend.Controllers;
using infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using service.Interfaces;
using service.Interfaces.Blob;
using Xunit;

public class Tests
{
    [Fact]
    public async Task GetBurgerById_ReturnsOk_WithBurger()
    {
        // Arrange
        var mockService = new Mock<IBurgerService>();
        var mockBlobService = new Mock<IBlobStorageService>();
        var testBurger = new Burger { id = 1, name = "Test Burger", price = 9.99m };

        mockService.Setup(service => service.GetBurgerById(1)).ReturnsAsync(testBurger);
        var controller = new BurgerController(mockService.Object, mockBlobService.Object);

        // Act
        var result = await controller.GetBurgerById(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<Burger>>(result).Result as OkObjectResult;
        var burgerResult = Assert.IsType<Burger>(okResult.Value);

        // Compare individual properties
        Assert.Equal(testBurger.id, burgerResult.id);
        Assert.Equal(testBurger.name, burgerResult.name);
        Assert.Equal(testBurger.price, burgerResult.price);
    }

    [Fact]
    public async Task GetBurgerById_ReturnsNotFound_WhenBurgerDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<IBurgerService>();
        var mockBlobService = new Mock<IBlobStorageService>();
        mockService.Setup(service => service.GetBurgerById(It.IsAny<int>())).ReturnsAsync((Burger)null);
        var controller = new BurgerController(mockService.Object, mockBlobService.Object);

        // Act
        var result = await controller.GetBurgerById(99);

        // Assert
        Assert.IsType<ActionResult<Burger>>(result);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetBurgers_ReturnsOk_WithListOfBurgers()
    {
        // Arrange
        var mockService = new Mock<IBurgerService>();
        var mockBlobService = new Mock<IBlobStorageService>();
        var mockBurgers = new List<Burger>
        {
            new Burger { id = 1, name = "Classic Burger", price = 5.99m },
            new Burger { id = 2, name = "Cheese Burger", price = 6.99m }
        };

        mockService.Setup(service => service.GetAllBurgers()).ReturnsAsync(mockBurgers);
        var controller = new BurgerController(mockService.Object, mockBlobService.Object);

        // Act
        var result = await controller.GetBurgers();

        // Assert
        var okResult = Assert.IsType<List<Burger>>(result);
        Assert.Equal(2, okResult.Count); // Verify the correct count

        // Compare individual properties of each burger
        Assert.Equal(mockBurgers[0].id, okResult[0].id);
        Assert.Equal(mockBurgers[0].name, okResult[0].name);
        Assert.Equal(mockBurgers[0].price, okResult[0].price);

        Assert.Equal(mockBurgers[1].id, okResult[1].id);
        Assert.Equal(mockBurgers[1].name, okResult[1].name);
        Assert.Equal(mockBurgers[1].price, okResult[1].price);
    }
}
