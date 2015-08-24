using System;

namespace SeleniumHelper
{
    public class PageObjectFactory
    {

        public static T Create<T>(string URL) where T : PageObject, new()
        {
            var Url = new Uri(URL);
            var product = new T();
            product.Url = Url;
            return product;
        }
    }
}