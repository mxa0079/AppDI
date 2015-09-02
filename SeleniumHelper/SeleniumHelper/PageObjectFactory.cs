using System;
using System.Configuration;

namespace SeleniumHelper
{
    public class PageObjectFactory
    {

        public static T Create<T>(string URL = "") where T : PageObject, new()
        {
            var product = new T();

            configurBaseUrl(product, URL);

            return product;
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