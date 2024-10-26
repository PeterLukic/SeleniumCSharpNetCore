using AventStack.ExtentReports;
using NUnit.Framework;
using SeleniumCSharpNetCore.Pages;
using System.Threading;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SeleniumCSharpNetCore.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly DriverHelper _driverHelper;
        private readonly HomePage homePage;
        private readonly LoginPage loginPage;
        private static readonly ThreadLocal<ExtentTest> _test = new ThreadLocal<ExtentTest>();

        public LoginSteps(DriverHelper driverHelper, ScenarioContext scenarioContext)
        {
            _driverHelper = driverHelper;
            homePage = new HomePage(_driverHelper.Driver);
            loginPage = new LoginPage(_driverHelper.Driver);

            // Get the Extent Test instance from the ScenarioContext, if available
            if (scenarioContext.ContainsKey("ExtentTest"))
            {
                _test.Value = (ExtentTest)scenarioContext["ExtentTest"];
            }
        }

        [Given(@"I navigate to application")]
        public void GivenINavigateToApplication()
        {
            _driverHelper.Driver.Navigate().GoToUrl("http://eaapp.somee.com/");
            _test.Value.Log(Status.Info, "Navigated to application.");
        }

        [Given(@"I click the Login link")]
        public void GivenIClickTheLoginLink()
        {
            homePage.ClickLogin();
            _test.Value.Log(Status.Info, "Clicked the Login link.");
        }

        [Then(@"I click the Login link failed")]
        public void ThenIClickTheLoginLinkFailed()
        {
            homePage.ClickLoginFailed();
            _test.Value.Log(Status.Warning, "Login link click failed.");
        }

        [Given(@"I enter username and password")]
        public void GivenIEnterUsernameAndPassword(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            loginPage.EnterUserNameAndPassword(data.UserName, data.Password);
            _test.Value.Log(Status.Info, "Entered username and password.");
        }

        [Given(@"I click login")]
        public void GivenIClickLogin()
        {
            loginPage.ClickLogin();
            _test.Value.Log(Status.Info, "Clicked login.");
        }

        [Then(@"I should see user logged in to the application")]
        public void ThenIShouldSeeUserLoggedInToTheApplication()
        {
            Assert.That(homePage.IsLogOffExist(), Is.True, "Log off button did not display.");
            _test.Value.Log(Status.Pass, "User successfully logged in to the application.");
        }
    }
}
