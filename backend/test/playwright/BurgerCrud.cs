using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace test.playwright;

[TestFixture]
public class BurgerCrud
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IPage _page;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
        _page = await _browser.NewPageAsync();
        Helper.TriggerRebuild();
    }

    [TestCase("Test Burger", 70, "Test Description")]
    public async Task BurgerCrudTest(string name, decimal price, string description)
    {
        await LoginAsAdmin();
        await NavigateToItemManagement();

        // Create
        await CreateBurgerTask(name, price, description);
        await VerifySuccessToast("Burger created successfully!");
        ClassicAssert.IsTrue(await VerifyBurgerExists(name), "Burger not found after creation.");

        // Update
        string updatedName = "Updated" + name;
        string updatedDescription = "Updated" + description;
        await SelectBurger(name);
        await UpdateBurger(updatedName, price, updatedDescription);
        await VerifySuccessToast("Burger updated successfully!");
        ClassicAssert.IsTrue(await VerifyBurgerExists(updatedName), "Burger not found after update.");

        // Delete
        await DeleteBurger(updatedName);
        await VerifySuccessToast("Burger deleted successfully!");
        ClassicAssert.IsFalse(await VerifyBurgerExists(updatedName), "Burger found after deletion.");
        
        Console.WriteLine("'Burger CRUD playwright tests' completed successfully.");
    }

    private async Task LoginAsAdmin()
    {
        await _page.GotoAsync($"{Helper.ClientBaseUrl}login");
        await _page.FillAsync("#usernameInput", "Sohaib");
        await _page.FillAsync("#passwordInput", "burger");
        await _page.ClickAsync("#loginButton");
    }

    private async Task NavigateToItemManagement()
    {
        await _page.ClickAsync("#menuButton");
        await _page.ClickAsync("#itemManagementButton");
    }

    private async Task CreateBurgerTask(string name, decimal price, string description)
    {
        await _page.ClickAsync("#createItemButton");
        await _page.SelectOptionAsync("#selectType", "Burger");
        await _page.FillAsync("#nameInput", name);
        await _page.FillAsync("#priceInput", price.ToString());
        await _page.FillAsync("#descriptionInput", description);

        var filePath = "/Users/mackbookair/Developer/Angular/CraftBurger-Web/test_image.jpg";
        if (await _page.Locator("#fileInput").IsVisibleAsync())
        {
            await _page.SetInputFilesAsync("#fileInput", filePath);
        }
        else
        {
            Assert.Fail("File input element not found.");
        }

        await _page.ClickAsync("#saveButton");
        await Task.Delay(TimeSpan.FromSeconds(2)); 
    }
    
    private async Task UpdateBurger(string newName, decimal newPrice, string newDescription)
    {
        await _page.ClickAsync("#editItemButton");
        await _page.FillAsync("#nameInput", newName);
        await _page.FillAsync("#priceInput", newPrice.ToString());
        await _page.FillAsync("#descriptionInput", newDescription);

        var filePath = "/Users/mackbookair/Developer/Angular/CraftBurger-Web/test_image.jpg";
        if (await _page.Locator("#fileInput").IsVisibleAsync())
        {
            await _page.SetInputFilesAsync("#fileInput", filePath);
        }
        else
        {
            Assert.Fail("File input element not found.");
        }
        
        await _page.ClickAsync("#saveButton");
        await Task.Delay(TimeSpan.FromSeconds(2)); 
    }

    private async Task DeleteBurger(string name)
    {
        await SelectBurger(name);
        await _page.ClickAsync("#deleteItemButton");
        await Task.Delay(TimeSpan.FromSeconds(2));
    }

    private async Task VerifySuccessToast(string message)
    {
        var toastMessageLocator = _page.Locator($"text='{message}'");
        await toastMessageLocator.WaitForAsync(new() { Timeout = 5000 });
    }
    
    private async Task SelectBurger(string name)
    {
        var itemRowLocator = _page.Locator($"table tr >> text='{name}'");

        await itemRowLocator.WaitForAsync(new() { Timeout = 5000 });

        if (await itemRowLocator.IsVisibleAsync())
        {
            await itemRowLocator.ClickAsync();
        }
        else
        {
            Assert.Fail($"Burger with name '{name}' not found in the table.");
        }
    }

    private async Task<bool> VerifyBurgerExists(string name)
    {
        var itemRowLocator = _page.Locator($"table tr >> text='{name}'");
        bool burgerExists = false;
        int attempts = 0;

        while (!burgerExists && attempts < 5)
        {
            burgerExists = await itemRowLocator.IsVisibleAsync();
            if (!burgerExists)
            {
                await Task.Delay(1000);
                attempts++;
            }
        }

        return burgerExists;
    }

    [TearDown]
    public async Task TearDown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }
}
