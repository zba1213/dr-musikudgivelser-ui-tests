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
        using var browser = CreateBrowser();
        OpenRecordsPage(browser);
        WaitForRows(browser, "1", "2", "3");

        AssertRow(browser, 1, "Hej Matematik", "Hej Matematik", "212", "2007");
        AssertRow(browser, 2, "Hjertestarter", "Nephew", "281", "2012");
        AssertRow(browser, 3, "Malibu", "Anderson .Paak", "3692", "2016");
    }

    [TestMethod]
    public void RecordsPage_FiltersByTitleAndArtistAndResets()
    {
        using var browser = CreateBrowser();
        OpenRecordsPage(browser);
        WaitForRows(browser, "1", "2", "3");

        var title = browser.FindElement(By.Id("title"));
        var artist = browser.FindElement(By.Id("artist"));
        var search = browser.FindElement(By.CssSelector("#app button[type=submit]"));
        var reset = browser.FindElement(By.CssSelector("#app button[type=button]"));

        title.SendKeys("  mAL  ");
        search.Click();
        WaitForRows(browser, "3");
        AssertRow(browser, 3, "Malibu", "Anderson .Paak", "3692", "2016");

        artist.SendKeys("nEp");
        search.Click();
        var wait = new WebDriverWait(browser, TimeSpan.FromSeconds(15));
        wait.Until(driver => driver.FindElements(By.CssSelector("#app [role=status]"))
            .Any(status => status.Text == "Ingen udgivelser matcher din søgning"));

        title.SendKeys(Keys.Control + "a" + Keys.Null + Keys.Backspace);
        search.Click();
        WaitForRows(browser, "2");
        AssertRow(browser, 2, "Hjertestarter", "Nephew", "281", "2012");

        // Begge felter skal ryddes, også når søgningen har to filtre.
        title.SendKeys("hjert");
        reset.Click();
        WaitForRows(browser, "1", "2", "3");
        Assert.AreEqual("", title.GetDomProperty("value"));
        Assert.AreEqual("", artist.GetDomProperty("value"));
    }

    private static ChromeDriver CreateBrowser()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        return new ChromeDriver(options);
    }

    private static void OpenRecordsPage(IWebDriver browser)
    {
        var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:5500";
        browser.Navigate().GoToUrl(frontendUrl);
    }

    private static void WaitForRows(IWebDriver browser, params string[] expectedIds)
    {
        var wait = new WebDriverWait(browser, TimeSpan.FromSeconds(15));
        wait.Message = $"Ventede på rækkerne {string.Join(", ", expectedIds)}.";
        wait.Until(driver => driver.FindElements(By.CssSelector("#app tbody tr"))
            .Where(row => row.Displayed)
            .Select(row => row.GetDomAttribute("data-record-id"))
            .SequenceEqual(expectedIds));
    }

    private static void AssertRow(IWebDriver browser, int id, params string[] expected)
    {
        var cells = browser.FindElements(By.CssSelector($"#app tr[data-record-id='{id}'] td"));
        CollectionAssert.AreEqual(expected, cells.Select(cell => cell.Text).ToArray());
    }
}
