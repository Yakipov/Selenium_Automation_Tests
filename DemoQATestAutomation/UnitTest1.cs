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

            // Ожидание загрузки страницы
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

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