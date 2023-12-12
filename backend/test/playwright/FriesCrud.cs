using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace test.playwright;

[TestFixture]
public class FriesCrud
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

    [TestCase("Test Fries", 50)]
    public async Task FriesCrudTest(string name, decimal price)
    {
        await LoginAsAdmin();
        await NavigateToItemManagement();

        // Create
        await CreateFriesTask(name, price);
        await VerifySuccessToast("Fries created successfully!");
        ClassicAssert.IsTrue(await VerifyFriesExists(name), "Fries not found after creation.");

        // Update
        string updatedName = "Updated" + name;
        await SelectFries(name);
        await UpdateFries(updatedName, price);
        await VerifySuccessToast("Fries updated successfully!");
        ClassicAssert.IsTrue(await VerifyFriesExists(updatedName), "Fries not found after update.");

        // Delete
        await DeleteFries(updatedName);
        await VerifySuccessToast("Fries deleted successfully!");
        ClassicAssert.IsFalse(await VerifyFriesExists(updatedName), "Fries found after deletion.");
        
        Console.WriteLine("'Fries CRUD playwright tests' completed successfully.");
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

    private async Task CreateFriesTask(string name, decimal price)
    {
        await _page.ClickAsync("#createItemButton");
        await _page.SelectOptionAsync("#selectType", "Fries");
        await _page.FillAsync("#nameInput", name);
        await _page.FillAsync("#priceInput", price.ToString());

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

    private async Task UpdateFries(string newName, decimal newPrice)
    {
        await _page.ClickAsync("#editItemButton");
        await _page.FillAsync("#nameInput", newName);
        await _page.FillAsync("#priceInput", newPrice.ToString());

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

    private async Task DeleteFries(string name)
    {
        await SelectFries(name);
        await _page.ClickAsync("#deleteItemButton");
        await Task.Delay(TimeSpan.FromSeconds(2));
    }
    
    private async Task VerifySuccessToast(string message)
    {
        var toastMessageLocator = _page.Locator($"text='{message}'");
        await toastMessageLocator.WaitForAsync(new() { Timeout = 5000 });
    }
    
    private async Task SelectFries(string name)
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
    
    private async Task<bool> VerifyFriesExists(string name)
    {
        var itemRowLocator = _page.Locator($"table tr >> text='{name}'");
        bool friesExists = false;
        int attempts = 0;

        while (!friesExists && attempts < 5)
        {
            friesExists = await itemRowLocator.IsVisibleAsync();
            if (!friesExists)
            {
                await Task.Delay(1000);
                attempts++;
            }
        }

        return friesExists;
    }

    [TearDown]
    public async Task TearDown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }
}
