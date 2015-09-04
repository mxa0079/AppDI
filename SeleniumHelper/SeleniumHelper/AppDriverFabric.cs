using System;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium;

namespace SeleniumHelper
{
    public class AppDriverFabric
    {
        private Uri _baseUrl;
        Lazy<IWebDriver> _webDriver;

        public AppDriverFabric()
        {

        }

        public AppDriverFabric(AppDriverFabric SourceappDriverFabric)
        {
            this._baseUrl = SourceappDriverFabric._baseUrl;
        }

        public AppDriver Create()
        {
            if(_baseUrl == null)
            {
                throw new MissingConfigurationException("The App Driver has not been properly configured. Missing URL and WebDriver type");
            }

            return new AppDriver(_baseUrl, _webDriver);
        }

        public AppDriverFabric Using<T>() where T : IWebDriver, new()
        {
            //Could not directly create a Lazy<T>, e.g. I could not directly convert Lazy<IEDriver> to Lazy<IWebDriver>
            //More here: http://stackoverflow.com/questions/4479334/casting-interface-type-in-lazyt
            return new AppDriverFabric(this) {_webDriver = new Lazy<IWebDriver>(() => new T())};
        }

        public AppDriverFabric Driving(string url)
        {
            var Url = new Uri(url);
            return new AppDriverFabric(this) { _baseUrl = Url};
        }
    }
}