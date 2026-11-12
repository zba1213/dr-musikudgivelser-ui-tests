using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

[assembly: DoNotParallelize]

namespace MusicRecords.UiTests;

[TestClass]
public class RecordsPageTests
{
    [TestMethod]
    public void RecordsPage_ShowsAllExampleRecords()
    {
        var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:5500";
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        using var browser = new ChromeDriver(options);
        browser.Navigate().GoToUrl(frontendUrl);

        var wait = new WebDriverWait(browser, TimeSpan.FromSeconds(15));
        wait.Message = "Ventede på de tre musikudgivelser i tabellen.";
        wait.Until(driver => driver.FindElements(By.CssSelector("#app tbody tr"))
            .Count(row => row.Displayed) == 3);

        AssertRow(browser, 1, "Hej Matematik", "Hej Matematik", "212", "2007");
        AssertRow(browser, 2, "Hjertestarter", "Nephew", "281", "2012");
        AssertRow(browser, 3, "Malibu", "Anderson .Paak", "3692", "2016");
    }

    private static void AssertRow(IWebDriver browser, int id, params string[] expected)
    {
        var cells = browser.FindElements(By.CssSelector($"#app tr[data-record-id='{id}'] td"));
        CollectionAssert.AreEqual(expected, cells.Select(cell => cell.Text).ToArray());
    }
}
