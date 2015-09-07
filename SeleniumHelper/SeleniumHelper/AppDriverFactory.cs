using System;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium;
using System.IO;
using Newtonsoft.Json;
using SeleniumHelper.Configuration;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using System.Reflection;

namespace SeleniumHelper    
{
    public class AppDriverFactory
    {
        private const string JsonConfigFileName = "appdi.config.json";
        private AppFabricConfig _jsonconfig;

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
            
            _jsonconfig = loadJsonConfiguration();

            this._baseUrl = this._baseUrl ?? new Uri(_jsonconfig.BaseUrl);
                
            this._webDriver = this._webDriver ?? extractDriverConfig(_jsonconfig.Browser);

            if(_baseUrl == null)
            {
                throw new MissingConfigurationException("The App Driver has not been properly configured. Missing BaseUrl. you can configure one by calling the \"Driving()\" method of the AppDriverFactory or by creating an appdi.config.json file at the root of your project.");
            }

            return new AppDriver(_baseUrl, _webDriver);
        }

        private AppFabricConfig loadJsonConfiguration()
        {
            AppFabricConfig jsonConfig = null;

            if (File.Exists(JsonConfigFileName))
            {
                using (StreamReader r = new StreamReader(JsonConfigFileName))
                {
                    string jsonString = r.ReadToEnd();
                    jsonConfig = JsonConvert.DeserializeObject<AppFabricConfig>(jsonString);
                }
            }

            return jsonConfig;
        }

        private Lazy<IWebDriver> extractDriverConfig(WebDrivers desiredDriver)
        {
            switch (desiredDriver)
            {
                case WebDrivers.Chrome:
                    return new Lazy<IWebDriver>(() => new ChromeDriver());
                case WebDrivers.Firefox:
                    return new Lazy<IWebDriver>(() => new FirefoxDriver());
                case WebDrivers.IE:
                    return new Lazy<IWebDriver>(() => new InternetExplorerDriver());
                case WebDrivers.PhantomJS:
                    return new Lazy<IWebDriver>(() => new PhantomJSDriver());
                case WebDrivers.Edge:
                    return new Lazy<IWebDriver>(() => new EdgeDriver());
                default:
                    return null;
            }
        }

        public AppDriverFactory Using<T>() where T : IWebDriver, new()
        {
            //Could not directly create a Lazy<T>, e.g. I could not directly convert Lazy<IEDriver> to Lazy<IWebDriver>
            //More here: http://stackoverflow.com/questions/4479334/casting-interface-type-in-lazyt
            return new AppDriverFactory(this) { _webDriver = new Lazy<IWebDriver>(() => {
                try
                {
                    return new T();

                }
                catch(TargetInvocationException te)
                {
                    throw te.InnerException;
                }
            })};
        }

        public AppDriverFactory Driving(string url)
        {
            var Url = new Uri(url);
            return new AppDriverFactory(this) { _baseUrl = Url};
        }
    }
}