using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;


namespace SeleniumCSharpNetCore
{
    public static class CustomHelpers
    {
        public static IWebElement WaitUntilClickable(this IWebDriver driver, By by, TimeSpan timeout)
        {
            var wait = new WebDriverWait(driver, timeout);
            return wait.Until(driver =>
            {
                var element = driver.FindElement(by);
                return element.Displayed && element.Enabled ? element : null;
            });
        }

        public static IWebElement WaitUntilVisible(this IWebDriver driver, By by, TimeSpan timeout)
        {
            var wait = new WebDriverWait(driver, timeout);
            return wait.Until(driver =>
            {
                var element = driver.FindElement(by);
                return element.Displayed ? element : null;
            });
        }
    }
}
