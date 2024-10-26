using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;


namespace SeleniumCSharpNetCore
{
    public class CustomControl: DriverHelper
    {

        private readonly IWebDriver _driver;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        public CustomControl(IWebDriver driver)
        {
            _driver = driver;
        }

        public void ComboBox(string controlName, string value)
        {
            try
            {
                // Locate the combo box input field and wait for it to be visible
                IWebElement comboControl = _driver.WaitUntilVisible(By.XPath($"//input[@id='{controlName}-awed']"), _timeout);
                comboControl.Clear();
                comboControl.SendKeys(value);

                // Wait for the dropdown options to be visible
                IWebElement optionToSelect = _driver.WaitUntilVisible(By.XPath($"//div[@id='{controlName}-dropmenu']//li[text()='{value}']"), _timeout);

                // Click on the desired option
                optionToSelect.Click();
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Element not found: {ex.Message}");
            }
            catch (ElementClickInterceptedException ex)
            {
                Console.WriteLine($"Click intercepted: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while selecting the combo box value: {ex.Message}");
            }
        }

        public static void EnterText(IWebElement webElement, string value) => webElement.SendKeys(value);

        public static void Click(IWebElement webElement) => webElement.Click();

        public static void SelectByValue(IWebElement webElement, string value)
        {
            SelectElement selectElement = new SelectElement(webElement);
            selectElement.SelectByValue(value);
        }

        public static void SelectByText(IWebElement webElement, string text)
        {
            SelectElement selectElement = new SelectElement(webElement);
            selectElement.SelectByText(text);
        }


    }
}
