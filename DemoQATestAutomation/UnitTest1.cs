using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace DemoQATestAutomation
{
    public class Tests
    {
        static IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console()
                 .WriteTo.File("logs.txt")
                 .CreateLogger();

            // Configure ChromeOptions to record browser logs
            ChromeOptions options = new ChromeOptions();
            options.SetLoggingPreference(LogType.Browser, LogLevel.All);

            // Initialize the Chrome driver using settings
            driver = new ChromeDriver(options);

            // Start of the test - recording the log
            Log.Information("The test has started.\n");
        }

        [Test]
        public void Test1()
        {

            Log.Information("Start test elements: Text box");

            driver.Navigate().GoToUrl("https://demoqa.com/elements"); 
            driver.Manage().Window.Maximize();

            // Search and click on an element
            InteractWithElementByXPath("//li[@id='item-0' and span[text()='Text Box']]");

            // Wait for the element to load and write to the log
            WaitUntilElementIsLoaded(By.XPath("//div[@class='col-12 mt-4 col-md-6']"), TimeSpan.FromSeconds(10));

            InteractWithElementByXPath("//input[@placeholder='Full Name' and @type='text']", true, "Test Text");

            InteractWithElementByXPath("//input[@id='userEmail']", true, "yackipov.asset@mail.ru");

            InteractWithElementByXPath("//textarea[@placeholder='Current Address']", true, "Test text Current Address");

            InteractWithElementByXPath("//textarea[@id='permanentAddress']", true, "Test text Permanent Address");

            // Scroll to the bottom of the page
            IWebElement body = driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.End);

             
            //Actions actions = new Actions(driver);
            //IWebElement submitButton = driver.FindElement(By.XPath("//button[@id='submit']"));
            //actions.MoveToElement(submitButton).Perform();

            WaitUntilElementIsLoaded(By.XPath("//button[@id='submit']"), TimeSpan.FromSeconds(10));

            InteractWithElementByXPath("//button[@id='submit']");
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                // Waiting for the <div id="output" class="mt-4 row"> element to appear
                IWebElement outputDiv = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("output")));

                // Checking the presence of an element
                if (outputDiv != null)
                {
                    Log.Information("The <div id='output'> element was found.");

                    // ����� ������ � ���
                    Log.Information($"Name: {outputDiv.FindElement(By.Id("name")).Text}");
                    Log.Information($"Email: {outputDiv.FindElement(By.Id("email")).Text}");
                    Log.Information($"Current address: {outputDiv.FindElement(By.Id("currentAddress")).Text}");
                    Log.Information($"Permanent Address: {outputDiv.FindElement(By.Id("permanentAddress")).Text}");
                }
                else
                {
                    Log.Error("The <div id='output'> element was not found.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Log.Error("Timeout for waiting for the <div id='output'> element to appear.");
            }

            // Example of using browser logs
            var logs = driver.Manage().Logs.GetLog(LogType.Browser);
            foreach (var log in logs)
            {
                // Check if the log is an error, and if so, write it
                if (log.Level == LogLevel.Severe)
                {
                    Log.Error($"Browser Error: {log.Timestamp} - {log.Message}");
                }
            }
        }

        static void InteractWithElementByXPath(string xpath, bool isTextInput = false, string text = null)
        {
            // �����������
            Log.Information($"����� �������� �� XPath: {xpath}");

            try
            {
                // ����� �������� �� XPath
                IWebElement element = driver.FindElement(By.XPath(xpath));

                // �������� ������� ��������
                if (element != null)
                {
                    // �����������
                    Log.Information("������� ������.");

                    // ���� isTextInput ����� true, �� ������ ����� � ����
                    if (isTextInput && !string.IsNullOrEmpty(text))
                    {
                        // ���� ������ � ����
                        element.SendKeys(text);
                        Log.Information($"����� '{text}' ������� ������ � ���� {xpath}.");
                    }
                    else
                    {
                        // ���������� ����� �� ��������
                        element.Click();
                    }
                }
                else
                {
                    // ����������� ������, ���� ������� �� ������
                    Log.Error("������� �� ������.");
                }
            }
            catch (NoSuchElementException ex)
            {
                Log.Error($"������� �� ������: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error($"��������� ������: {ex.Message}");
            }
        }

        static void WaitUntilElementIsLoaded(By locator, TimeSpan timeout)
        {
            // �����������
            Log.Information($"�������� �������� �������� � �������������� ��������: {locator}");

            // �������� �������� ��������
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));

            // ����������� ��������� ���������� ��������
            Log.Information($"������� ������� ��������.");
        }

        [TearDown]
        public void TearDown() 
        {
            // ���������� ����� - ������ ����
            Log.Information("���� ����������.\n\n\n");

            // �������� ��������
            driver.Quit();

            // ������������ �������� �����������
            Log.CloseAndFlush();
        }
    }
}