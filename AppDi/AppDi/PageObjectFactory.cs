using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Configuration;

namespace AppDi
{
    public static class PageObjectFactory
    {
        private static IWebDriver WebDriver;

        public static T Create<T>(string URL = "") where T : PageObject, new()
        {
            var product = new T();

            configurBaseUrl(product, URL);

            configureWebDriver(product);

            return product;
        }

        private static void configureWebDriver<T>(T product) where T : PageObject, new()
        {
            initializeDriver();
            product.WebDriver = WebDriver;
        }

        private static void initializeDriver()
        {
            if (WebDriver == null)
                WebDriver = new PhantomJSDriver();
        }

        private static void configurBaseUrl<T>(T product, string url) where T : PageObject, new()
        {
            if (url == string.Empty)
            {
                product.Url = new Uri(ConfigurationManager.AppSettings["BaseUrl"].ToString());
            }
            else
            {
                product.Url = new Uri(url);
            }
        }
    }
}