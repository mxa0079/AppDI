using System;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium;
using System.IO;
using Newtonsoft.Json;
using AppDi.Configuration;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using System.Reflection;
using System.Dynamic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AppDi    
{
    public class AppDriverFactory
    {
        private const string JsonConfigFileName = "appdi.config.json";
        private AppFabricConfig _jsonconfig;

        private Dictionary<string, Type> _pages = new Dictionary<string, Type>();

        public AppDriverFactory Register<T>() where T : PageObject
        {
            var pageType = typeof(T);
            _pages.Add(cleanPageObjectsuffix(pageType.Name), pageType);
            return this;
        }

        public AppDriverFactory Register(string namespaceToLoad)
        {
            //If the desired namsepace to load is MyNameSpace.Pagebjects, then we assume the existance of a MyNameSpace.dll in the working folder
            //Generating name of assumed dll file:
            var topLevelNamespace = namespaceToLoad.Split('.')[0];

            var classesToRegister = AppDomain.CurrentDomain.GetAssemblies()
                                            .Where(t => t.GetName().Name == topLevelNamespace)
                                            .SelectMany(t => t.GetTypes())
                                            .Where(t => t.IsClass && t.Namespace == namespaceToLoad && t.BaseType == typeof(PageObject));


            //var workingdirectoryQueryResults = Directory.GetFiles(@".\", assumedDLLname);
            if (classesToRegister.Count() == 0)
            {
                throw new Exception("Error: could not find any Page Objects to register on namespace: " + namespaceToLoad + " AppDi looked through the loaded assemblies, assumed the assembly name to be " + topLevelNamespace + " However, either the namespace was not found or no classes were found within that namespace");
            };

            foreach(var pageObject in classesToRegister)
            {
                _pages.Add(cleanPageObjectsuffix(pageObject.Name), pageObject);
            }

            return this;
        }

        private string cleanPageObjectsuffix(string pageObjectName)
        {
            const string suffix = "PageObject";

            if (pageObjectName.EndsWith(suffix))
            {
                return pageObjectName.Substring(0, pageObjectName.Length - suffix.Length);
            }
            else
            {
                return pageObjectName;
            }
        }

        private Uri _baseUrl;
        Lazy<IWebDriver> _webDriver;
        Func<Uri, Lazy<IWebDriver>, Dictionary<string, Type>, AppDriver> AppDriverConstructor;

        /// <summary>
        /// AppDriverFactory should only be instantiated through the AppDriver.Factory() method
        /// </summary>
        /// <param name="func">
        /// Func is responsible for creating an instannce of AppDriver. We made the constructor of AppDriver private to ensure it is only created through its factory.
        /// </param>
        internal AppDriverFactory(Func<Uri, Lazy<IWebDriver>, Dictionary<string, Type>, AppDriver> func)
        {
            AppDriverConstructor = func;
        }

        //TODO: Not a big fan of this approach. Feels too brute force.
        //Since every time I add a new property to the Factory, I need to update this cloning code. Granted... AppDriverFactory should seldomly change
        public AppDriverFactory(AppDriverFactory sourceAppDriverFactory)
        {
            AppDriverConstructor = sourceAppDriverFactory.AppDriverConstructor;

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

            if(_jsonconfig != null)
            {
                this._baseUrl = this._baseUrl ?? new Uri(_jsonconfig.BaseUrl);
                this._webDriver = this._webDriver ?? extractDriverConfig(_jsonconfig.Browser);
            }
            else
            {
                if (_baseUrl == null)
                {
                    throw new MissingConfigurationException("The App Driver has not been properly configured. Missing BaseUrl. you can configure one by calling the \"Driving()\" method of the AppDriverFactory or by creating an appdi.config.json file at the root of your project.");
                }

                this._webDriver = new Lazy<IWebDriver>(() => new FirefoxDriver());
            }

            //Replace with this: http://stackoverflow.com/questions/515269/factory-pattern-in-c-how-to-ensure-an-object-instance-can-only-be-created-by-a
            return AppDriverConstructor(_baseUrl, _webDriver, _pages);
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