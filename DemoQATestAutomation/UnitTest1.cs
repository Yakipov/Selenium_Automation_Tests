using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using OpenQA.Selenium.Support.UI;

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

            // ��������� ChromeOptions ��� ������ ����� ��������
            ChromeOptions options = new ChromeOptions();
            options.SetLoggingPreference(LogType.Browser, LogLevel.All);

            // ������������� �������� Chrome � �������������� ��������
            driver = new ChromeDriver(options);

            // ������ ����� - ������ ����
            Log.Information("���� �������.");
        }

        [Test]
        public void Test1()
        {

            driver.Navigate().GoToUrl("https://demoqa.com/elements"); 
            driver.Manage().Window.Maximize();

            // ����� � ���� �� �������� "Text Box"
            ClickElementByXPath("//li[@id='item-0' and span[text()='Text Box']]");

            // �������� �������� ��������
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // ������ ������������� ����� ��������
            var logs = driver.Manage().Logs.GetLog(LogType.Browser);
            foreach (var log in logs)
            {
                // ���������, �������� �� ��� �������, � ���� ��, ���������� ���
                if (log.Level == LogLevel.Severe)
                {
                    Log.Error($"Browser Error: {log.Timestamp} - {log.Message}");
                }
            }
        }

        static void ClickElementByXPath(string xpath)
        {
            // �����������
            Log.Information($"����� �������� �� XPath: {xpath}");

            // ����� �������� �� XPath
            IWebElement element = driver.FindElement(By.XPath(xpath));

            // �������� ������� ��������
            if (element != null)
            {
                // �����������
                Log.Information("������� ������.");

                // ���������� ����� �� ��������
                element.Click();
            }
            else
            {
                // ����������� ������, ���� ������� �� ������
                Log.Error("������� �� ������.");
            }
        }

        [TearDown]
        public void TearDown() 
        {
            // ���������� ����� - ������ ����
            Log.Information("���� ����������.");

            // �������� ��������
            driver.Quit();

            // ������������ �������� �����������
            Log.CloseAndFlush();
        }
    }
}