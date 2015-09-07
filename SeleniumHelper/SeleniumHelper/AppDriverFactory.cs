using System;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium;

namespace SeleniumHelper
{
    public class AppDriverFactory
    {
        private Uri _baseUrl;
        Lazy<IWebDriver> _webDriver;

        public AppDriverFactory()
        {

        }

        public AppDriverFactory(AppDriverFactory sourceAppDriverFactory)
        {
            if(sourceAppDriverFactory._baseUrl != null)
            {
                this._baseUrl = sourceAppDriverFactory._baseUrl;
            }

            if(sourceAppDriverFactory._webDriver != null)
            {
                this._webDriver = sourceAppDriverFactory._webDriver;
            }
        }

        public AppDriver Create()
        {
            if(_baseUrl == null)
            {
                throw new MissingConfigurationException("The App Driver has not been properly configured. Missing URL and WebDriver type");
            }

            return new AppDriver(_baseUrl, _webDriver);
        }

        public AppDriverFactory Using<T>() where T : IWebDriver, new()
        {
            //Could not directly create a Lazy<T>, e.g. I could not directly convert Lazy<IEDriver> to Lazy<IWebDriver>
            //More here: http://stackoverflow.com/questions/4479334/casting-interface-type-in-lazyt
            return new AppDriverFactory(this) {_webDriver = new Lazy<IWebDriver>(() => new T())};
        }

        public AppDriverFactory Driving(string url)
        {
            var Url = new Uri(url);
            return new AppDriverFactory(this) { _baseUrl = Url};
        }
    }
}