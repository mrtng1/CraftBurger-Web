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
        var testBurger = new Burger { ID = 1, BurgerName = "Test Burger", BurgerPrice = 9.99m };

        mockService.Setup(service => service.GetBurgerById(1)).ReturnsAsync(testBurger);
        var controller = new BurgerController(mockService.Object, mockBlobService.Object);

        // Act
        var result = await controller.GetBurgerById(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<Burger>>(result).Result as OkObjectResult;
        var burgerResult = Assert.IsType<Burger>(okResult.Value);

        // Compare individual properties
        Assert.Equal(testBurger.ID, burgerResult.ID);
        Assert.Equal(testBurger.BurgerName, burgerResult.BurgerName);
        Assert.Equal(testBurger.BurgerPrice, burgerResult.BurgerPrice);
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
            new Burger { ID = 1, BurgerName = "Classic Burger", BurgerPrice = 5.99m },
            new Burger { ID = 2, BurgerName = "Cheese Burger", BurgerPrice = 6.99m }
        };

        mockService.Setup(service => service.GetAllBurgers()).ReturnsAsync(mockBurgers);
        var controller = new BurgerController(mockService.Object, mockBlobService.Object);

        // Act
        var result = await controller.GetBurgers();

        // Assert
        var okResult = Assert.IsType<List<Burger>>(result);
        Assert.Equal(2, okResult.Count); // Verify the correct count

        // Compare individual properties of each burger
        Assert.Equal(mockBurgers[0].ID, okResult[0].ID);
        Assert.Equal(mockBurgers[0].BurgerName, okResult[0].BurgerName);
        Assert.Equal(mockBurgers[0].BurgerPrice, okResult[0].BurgerPrice);

        Assert.Equal(mockBurgers[1].ID, okResult[1].ID);
        Assert.Equal(mockBurgers[1].BurgerName, okResult[1].BurgerName);
        Assert.Equal(mockBurgers[1].BurgerPrice, okResult[1].BurgerPrice);
    }
}
