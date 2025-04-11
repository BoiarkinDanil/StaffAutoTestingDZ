using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace StaffTests;

public class Tests
{
    private IWebDriver _driver;
    private WebDriverWait _wait;
    private const string staffUrl = "https://staff-testing.testkontur.ru";


    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensins");

        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        Login("danilaboyarkin4622@gmail.com", "77zapobU!77");
    }
    private void Login(string username, string password)
    {
        _driver.Navigate().GoToUrl(staffUrl);

        var usernameField = _driver.FindElement(By.Id("Username"));
        usernameField.SendKeys(username);

        var passwordField = _driver.FindElement(By.Id("Password"));
        passwordField.SendKeys(password);

        _driver.FindElement(By.Name("button")).Click();

        _wait.Until(ExpectedConditions.UrlToBe($"{staffUrl}/news"));
    }

    [Test]
    public void AuthorizationTest()
    {
        var titlePageElement = _driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(_driver.Title, Is.EqualTo("Новости"), "Ошибка авторизации");
    }

    [Test]
    public void SearchBarProfileTest()
    {
        var searchBar = _driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        searchBar.Click();

        var input = searchBar.FindElement(By.ClassName("react-ui-1oilwm3"));
        input.SendKeys("Абышева Светлана Олеговна");

        var dropdownList = _wait.Until(x => x.FindElement(By.CssSelector("[data-tid='ScrollContainer__inner']")));

        dropdownList.Click();

        Assert.That(_driver.Url, Is.EqualTo($"{staffUrl}/profile/bb00869f-f4c5-4e3e-b2dc-c2087a1e71e2"), "Страница не найдена");
    }

    [Test]
    public void CreateNewCommunityTest()
    {
        _driver.Navigate().GoToUrl($"{staffUrl}/communities");

        var ButtonCreateCommunity = _wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//section[@data-tid='PageHeader']//button[contains(., 'СОЗДАТЬ')]")));
        ButtonCreateCommunity.Click();

        var CommunityName = _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Name']")));
        CommunityName.SendKeys("New community");

        var ButtonCreate = _driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        ButtonCreate.Click();

        var communityManagement = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("span[data-tid='Title']")));
        Assert.That(communityManagement.Text, Does.Contain("Управление сообществом"), "Ожидалось, что содержит 'Управление сообществом'");
    }

    [Test]
    public void NavigationMenuElementEventsTest()
    {
        var NavigationEventButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Events']")));
        NavigationEventButton.Click();

        Assert.That(_driver.Url, Is.EqualTo($"{staffUrl}/events"), "Страница не найдена");
    }

    [Test]
    public void CreateNewEventTest()
    {
        _driver.Navigate().GoToUrl($"{staffUrl}/events");

        var ButtonCreateEvent = _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='AddButton']")));
        ButtonCreateEvent.Click();

        var EventName = _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Name']")));
        EventName.SendKeys("New Event");

        var INN = _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[placeholder='Введите ИНН']")));
        INN.SendKeys("755355252400");

        var AllDay = _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='AllDay']")));
        AllDay.Click();

        var ButtonCreate = _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='CreateButton']")));
        ButtonCreate.Click();

        var EventEditor = _driver.FindElement(By.CssSelector("[data-tid='SettingsTabWrapper']"));

        var communityManagement = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("span[data-tid='Title']")));
        Assert.That(communityManagement.Text, Does.Contain("Управление мероприятием"), "Ожидалось, что содержит 'Управление мероприятием'");
    }


    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
