using AppDi;
using System;
using Xunit;

namespace AppDiTests
{

    using AppDiTests.VanillaPageObjects;

    /// <summary>
    /// Holds all tests related to base url configuration
    /// </summary>
    public class PFUrlConfigurationTests : IDisposable
    {
        ConcretePageObject FactoryResult = null;

        [Fact]
        public void Creates_PageObject_With_Explicit_Base_URL()
        {
            FactoryResult = PageObjectFactory.Create<ConcretePageObject>("http://www.bing.com");

            Assert.Equal<string>("http://www.bing.com/", FactoryResult.Url.ToString());
        }

        [Fact]
        public void Creates_PageObject_With_Config_Based_Url()
        {
            //Since no URL is provided, PageFactory will try to read it from config file
            FactoryResult = PageObjectFactory.Create<ConcretePageObject>();

            Assert.Equal<string>("http://www.google.com/", FactoryResult.Url.ToString());
        }

        public void Dispose()
        {
            if(FactoryResult != null)
                FactoryResult.WebDriver.Quit();
        }
    }
}
