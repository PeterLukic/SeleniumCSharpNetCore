using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumCSharpNetCore.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            // Set up an explicit wait with a 10-second timeout
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // Use custom wait methods to initialize web elements
        private IWebElement TxtUserName => WaitUntilVisible(By.Name("UserName"));
        private IWebElement TxtPassword => WaitUntilVisible(By.Name("Password"));
        private IWebElement BtnLogin => WaitUntilClickable(By.CssSelector(".btn-default"));

        private IWebElement WaitUntilClickable(By by)
        {
            return _wait.Until(driver =>
            {
                var element = driver.FindElement(by);
                return element.Displayed && element.Enabled ? element : null;
            });
        }

        private IWebElement WaitUntilVisible(By by)
        {
            return _wait.Until(driver =>
            {
                var element = driver.FindElement(by);
                return element.Displayed ? element : null;
            });
        }

        public void EnterUserNameAndPassword(string userName, string password)
        {
            try
            {
                TxtUserName.Clear(); // Clear any pre-filled text
                TxtUserName.SendKeys(userName);
                TxtPassword.Clear(); // Clear any pre-filled text
                TxtPassword.SendKeys(password);
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Error finding username/password field: {ex.Message}");
            }
            catch (ElementNotInteractableException ex)
            {
                Console.WriteLine($"Username/password field not interactable: {ex.Message}");
            }
        }

        public void ClickLogin()
        {
            try
            {
                BtnLogin.Click();
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Error finding the login button: {ex.Message}");
            }
            catch (ElementClickInterceptedException ex)
            {
                Console.WriteLine($"Login button click intercepted: {ex.Message}");
            }
        }
    }
}
