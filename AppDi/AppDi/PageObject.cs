using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AppDi
{
    public abstract class PageObject
    {
        public WebDriverWait Wait;
        public IWebDriver WebDriver { get; set; }

        public Uri Url { get; set; }
    }
}