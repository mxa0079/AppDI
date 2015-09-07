using OpenQA.Selenium.Edge;
using OpenQA.Selenium.PhantomJS;
using AppDi;
using System;
using Xunit;

namespace AppDiTests.AppDriverFactoryTests
{
    public class AppDriverFactoryConfiguration
    {
        private AppDriverFactory SUT;

        private readonly string DESIRED_URL = "http://www.bing.com/";


        public AppDriverFactoryConfiguration()
        {
            SUT = new AppDriverFactory();
        }

        //Temporarily commenting out. Need to find a way to prevent the Factory from using the 
        //Json configuration so this exception will be thrown
        //[Fact]
        //[Trait("SUT", "AppDriverFactory")]
        //public void Creating_App_Driver_With_No_Config_Throws_Exception()
        //{
        //    Assert.Throws<MissingConfigurationException>(() => SUT.Create());
        //}

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        public void Create_App_Driver_With_Explicit_URL()
        {
            AppDriver result = SUT.Driving(DESIRED_URL).Create();

            Assert.NotNull(result.BaseUrl);
            Assert.Equal<string>(DESIRED_URL, result.BaseUrl.ToString());
        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        public void Creates_AppDriver_specifying_baseURL_fisrt_and_then_Browser()
        {
            //set base url first, browser second
            AppDriver result = SUT.Driving(DESIRED_URL).Using<PhantomJSDriver>().Create();

            Assert.NotNull(result.WebDriver);
            Assert.Equal(DESIRED_URL, result.BaseUrl.ToString());
            //Initially, I had the assertion below. But this is redundant.
            //My code is not in charge of selecting the correct IWebDriver, I use generics for that
            //I should let the .net take care of testing that part
            //Great example of thinking through your assertions
            //Assert.IsType<Lazy<PhantomJSDriver>>(result.WebDriver);
            //It also resulted in some nasty cleanup code:
            //result.WebDriver.Value.Quit();
        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        public void Creates_AppDriver_specifying_browser_first_and_then_baseURL()
        {
            //select browser first, set base url second
            AppDriver result = SUT.Using<PhantomJSDriver>().Driving(DESIRED_URL).Create();

            Assert.NotNull(result.WebDriver);
            Assert.Equal(DESIRED_URL, result.BaseUrl.ToString());
        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        public void Creates_AppDriver_taking_defaults_from_JSON_config()
        {
            const string JSON_CONFIG_BASE_URL = @"http://www.google.com/";

            AppDriver result = SUT.Create();

            Assert.NotNull(result.WebDriver);
            Assert.NotNull(result.BaseUrl);

            Assert.Equal(JSON_CONFIG_BASE_URL, result.BaseUrl.ToString());
        }

        //Removed because functionality tested is small, and test is unreliable due to its dependency on a WebDriver other than PhantomJS
        //[Fact]
        //[Trait("SUT", "AppDriverFactory")]
        //public void Gives_Preference_To_Code_Based_WebDriver_Configuration()
        //{
        //    AppDriver result = SUT.Using<EdgeDriver>().Create();

        //    //To simplify unit test dependencies
        //    //Instead of asserting for the type of EdgeDriver (which would require creating an instance of an edge driver)
        //    //I Expect that the DriverServiceNotFOundException to be thrown since I am trying to insta

        //    Assert.Throws<OpenQA.Selenium.DriverServiceNotFoundException>(() => result.WebDriver.Value);
        //}
    }
}
