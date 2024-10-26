using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumCSharpNetCore.Pages
{
    class HomePage
    {
        private IWebDriver Driver;

        public HomePage(IWebDriver driver)
        {
            Driver = driver;
        }

        IWebElement lnkLogin => Driver.FindElement(By.LinkText("Login"));

        IWebElement lnkLoginFailed => Driver.FindElement(By.LinkText("Login111"));

        IWebElement lnkLogOff => Driver.FindElement(By.LinkText("Log off"));

        public void ClickLogin() => lnkLogin.Click();

        public void ClickLoginFailed() => lnkLoginFailed.Click();

        public bool IsLogOffExist() => lnkLogOff.Displayed;
    }
}
