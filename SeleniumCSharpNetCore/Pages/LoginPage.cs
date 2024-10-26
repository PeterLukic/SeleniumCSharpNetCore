using OpenQA.Selenium;
using System;

namespace SeleniumCSharpNetCore.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement TxtUserName => _driver.WaitUntilVisible(By.Name("UserName"), _timeout);
        private IWebElement TxtPassword => _driver.WaitUntilVisible(By.Name("Password"), _timeout);
        private IWebElement BtnLogin => _driver.WaitUntilClickable(By.CssSelector(".btn-default"), _timeout);

        public void EnterUserNameAndPassword(string userName, string password)
        {
            try
            {
                TxtUserName.Clear();
                TxtUserName.SendKeys(userName);
                TxtPassword.Clear();
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
