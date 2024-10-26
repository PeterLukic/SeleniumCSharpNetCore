using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace SeleniumCSharpNetCore
{
    public class DriverHelper
    {
        private static ThreadLocal<IWebDriver> _driver = new ThreadLocal<IWebDriver>();

        public IWebDriver Driver
        {
            get
            {
                // If the driver is not initialized, create a new instance
                if (!_driver.Value.Equals(null))
                {
                    return _driver.Value;
                }

                _driver.Value = new ChromeDriver(); // Initialize the driver (or any other driver you need)
                return _driver.Value;
            }
            set
            {
                _driver.Value = value; // Allows manual driver assignment if needed
            }
        }
    }
}
