using api.Models;
using backend.Controllers;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using service;
using Xunit;

namespace test;

public class Tests
{
    [Fact]
    public async Task GetBurgerById_ReturnsOk_WithBurger()
    {
        // Arrange
        var mockService = new Mock<IBurgerService>();
        var testBurger = new Burger { ID = 1, BurgerName = "Test Burger", BurgerPrice = 9.99m };

        mockService.Setup(service => service.GetBurgerById(1)).ReturnsAsync(testBurger);
        var controller = new BurgerController(mockService.Object);

        // Act
        var result = await controller.GetBurgerById(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<Burger>>(result).Result as OkObjectResult;
        var burgerResult = Assert.IsType<Burger>(okResult.Value);
        Assert.Equal(testBurger, burgerResult);
    }

    [Fact]
    public async Task GetBurgerById_ReturnsNotFound_WhenBurgerDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<IBurgerService>();
        mockService.Setup(service => service.GetBurgerById(It.IsAny<int>())).ReturnsAsync((Burger)null);
        var controller = new BurgerController(mockService.Object);

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
        var mockBurgers = new List<Burger>
        {
            new Burger { ID = 1, BurgerName = "Classic Burger", BurgerPrice = 5.99m },
            new Burger { ID = 2, BurgerName = "Cheese Burger", BurgerPrice = 6.99m }
        };

        mockService.Setup(service => service.GetAllBurgers()).ReturnsAsync(mockBurgers);
        var controller = new BurgerController(mockService.Object);

        // Act
        var result = await controller.GetBurgers();

        // Assert
        var okResult = Assert.IsType<List<Burger>>(result);
        Assert.Equal(2, okResult.Count); // Verify the correct count
        Assert.Equal(mockBurgers, okResult); // Verify the actual data
    }
}