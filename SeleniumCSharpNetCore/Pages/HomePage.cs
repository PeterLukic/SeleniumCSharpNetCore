using OpenQA.Selenium;
using System;

namespace SeleniumCSharpNetCore.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement LnkLogin => _driver.WaitUntilClickable(By.LinkText("Login"), _timeout);
        private IWebElement LnkLoginFailed => _driver.WaitUntilClickable(By.LinkText("Login111"), _timeout);
        private IWebElement LnkLogOff => _driver.WaitUntilVisible(By.LinkText("Log off"), _timeout);

        public void ClickLogin()
        {
            try
            {
                LnkLogin.Click();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Login link not found.");
            }
            catch (ElementClickInterceptedException)
            {
                Console.WriteLine("Login link was not clickable.");
            }
        }

        public void ClickLoginFailed()
        {
            try
            {
                LnkLoginFailed.Click();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Login failed link not found.");
            }
            catch (ElementClickInterceptedException)
            {
                Console.WriteLine("Login failed link was not clickable.");
            }
        }

        public bool IsLogOffExist()
        {
            try
            {
                return LnkLogOff.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false; // Log off button doesn't exist
            }
        }
    }
}
