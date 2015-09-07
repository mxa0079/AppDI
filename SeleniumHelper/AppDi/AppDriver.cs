using OpenQA.Selenium;
using System;

namespace AppDi
{
    public class AppDriver
    {
        public Uri BaseUrl { get; }
        public Lazy<IWebDriver> WebDriver { get; }

        public AppDriver(Uri baseUrl, Lazy<IWebDriver> webDriver)
        {
            this.BaseUrl = baseUrl;
            this.WebDriver = webDriver;
        }
    }
}