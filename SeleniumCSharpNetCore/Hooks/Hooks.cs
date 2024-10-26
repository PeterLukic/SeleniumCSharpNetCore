using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;
[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace SeleniumCSharpNetCore.Hooks
{
    [Binding]
   // [Parallelizable(ParallelScope.Fixtures)]
    public sealed class Hooks
    {
        private readonly DriverHelper _driverHelper;
        private static ExtentReports _extent;
        private static ThreadLocal<ExtentTest> _test = new ThreadLocal<ExtentTest>();
        private static readonly ThreadLocal<ScenarioContext> _scenarioContext = new ThreadLocal<ScenarioContext>();

        public Hooks(DriverHelper driverHelper)
        {
            _driverHelper = driverHelper;
        }

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
            // Store ScenarioContext for later use
            _scenarioContext.Value = scenarioContext;

            // Set up Chrome options
            var option = new ChromeOptions();
            option.AddArguments("start-maximized");
            option.AddArguments("--disable-gpu");

            // Initialize the DriverHelper with a new instance of ChromeDriver
            _driverHelper.Driver = new ChromeDriver(option);

            // Create a new ExtentTest for this scenario
            _test.Value = _extent.CreateTest(scenarioContext.ScenarioInfo.Title);

            // Store the ExtentTest in ScenarioContext for access in step definitions
            scenarioContext["ExtentTest"] = _test.Value;
        }

        [AfterStep]
        public void InsertReportingSteps()
        {
            if (_test.Value == null)
            {
                Console.WriteLine("Warning: ExtentTest object is null. Ensure BeforeScenario is properly initializing the test.");
                return;
            }

            if (_scenarioContext.Value.TestError == null)
            {
                _test.Value.Log(Status.Pass, "Step passed");
            }
            else
            {
                var screenshotPath = CaptureScreenshot(_scenarioContext.Value.ScenarioInfo.Title);
                var mediaEntity = MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build();
                _test.Value.Log(Status.Fail, $"Step failed: {_scenarioContext.Value.TestError.Message}", mediaEntity);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driverHelper.Driver.Quit(); // Use the method to quit the driver
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
