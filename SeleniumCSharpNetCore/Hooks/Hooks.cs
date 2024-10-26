using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;

namespace SeleniumCSharpNetCore.Hooks
{
    [Binding]
    [Parallelizable(ParallelScope.Fixtures)]
    public sealed class Hooks
    {
        private DriverHelper _driverHelper;
        private static ExtentReports _extent;
        private static ThreadLocal<ExtentTest> _test = new();

        public Hooks(DriverHelper driverHelper) => _driverHelper = driverHelper;

        [BeforeTestRun]
        public static void InitializeReport()
        {
            var htmlReporter = new ExtentSparkReporter("extentReport.html");
            htmlReporter.Config.DocumentTitle = "Test Report";
            htmlReporter.Config.ReportName = "Selenium Test Report";

            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            ChromeOptions option = new ChromeOptions();
            option.AddArguments("start-maximized");
            option.AddArguments("--disable-gpu");

            var driver = new ChromeDriver(option);
            _driverHelper.Driver = driver;
            _test.Value = _extent.CreateTest(scenarioContext.ScenarioInfo.Title);
        }

        [AfterStep]
        public void InsertReportingSteps(ScenarioContext scenarioContext)
        {
            if (_test.Value == null)
            {
                Console.WriteLine("Warning: ExtentTest object is null. Ensure BeforeScenario is properly initializing the test.");
                return;
            }

            if (scenarioContext.TestError == null)
            {
                _test.Value.Log(Status.Pass, "Step passed");
            }
            else
            {
                var screenshotPath = CaptureScreenshot(scenarioContext.ScenarioInfo.Title);
                var mediaEntity = MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build();
                _test.Value.Log(Status.Fail, $"Step failed: {scenarioContext.TestError.Message}", mediaEntity);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driverHelper.Driver.Quit();
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            _extent.Flush();
        }

        private string CaptureScreenshot(string scenarioName)
        {
            var fileName = $"{scenarioName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            try
            {
                Screenshot screenshot = ((ITakesScreenshot)_driverHelper.Driver).GetScreenshot();
                screenshot.SaveAsFile(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error capturing screenshot: {ex.Message}");
            }

            return filePath;
        }
    }
}
