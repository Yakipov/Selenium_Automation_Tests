using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

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

            // Настройка ChromeOptions для записи логов браузера
            ChromeOptions options = new ChromeOptions();
            options.SetLoggingPreference(LogType.Browser, LogLevel.All);

            // Инициализация драйвера Chrome с использованием настроек
            driver = new ChromeDriver(options);

            // Начало теста - запись лога
            Log.Information("Тест начался.");
        }

        [Test]
        public void Test1()
        {

            driver.Navigate().GoToUrl("https://demoqa.com/elements"); 
            driver.Manage().Window.Maximize();

            // Поиск и клик на элементе "Text Box"
            ClickElementByXPath("//li[@id='item-0' and span[text()='Text Box']]");

            // Ожидание загрузки элемента и запись в лог
            WaitUntilElementIsLoaded(By.XPath("//div[@class='col-12 mt-4 col-md-6']"), TimeSpan.FromSeconds(10));

            // Пример использования логов браузера
            var logs = driver.Manage().Logs.GetLog(LogType.Browser);
            foreach (var log in logs)
            {
                // Проверяем, является ли лог ошибкой, и если да, записываем его
                if (log.Level == LogLevel.Severe)
                {
                    Log.Error($"Browser Error: {log.Timestamp} - {log.Message}");
                }
            }
        }

        static void ClickElementByXPath(string xpath)
        {
            // Логирование
            Log.Information($"Поиск элемента по XPath: {xpath}");

            // Поиск элемента по XPath
            IWebElement element = driver.FindElement(By.XPath(xpath));

            // Проверка наличия элемента
            if (element != null)
            {
                // Логирование
                Log.Information("Элемент найден.");

                // Выполнение клика на элементе
                element.Click();
            }
            else
            {
                // Логирование ошибки, если элемент не найден
                Log.Error("Элемент не найден.");
            }
        }


        static void WaitUntilElementIsLoaded(By locator, TimeSpan timeout)
        {
            // Логирование
            Log.Information($"Ожидание загрузки элемента с использованием локатора: {locator}");

            // Ожидание загрузки элемента
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));

            // Логирование успешного завершения ожидания
            Log.Information($"Элемент успешно загружен.");
        }

        [TearDown]
        public void TearDown() 
        {
            // Завершение теста - запись лога
            Log.Information("Тест завершился.");

            // Закрытие браузера
            driver.Quit();

            // Освобождение ресурсов логирования
            Log.CloseAndFlush();
        }
    }
}