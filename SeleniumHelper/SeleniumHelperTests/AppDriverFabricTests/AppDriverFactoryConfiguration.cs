using OpenQA.Selenium.PhantomJS;
using SeleniumHelper;
using System;
using Xunit;

namespace SeleniumHelperTests.AppDriverFactoryTests
{
    public class AppDriverFactoryConfiguration
    {
        private AppDriverFactory SUT;

        private readonly string DESIRED_URL = "http://www.bing.com/";


        public AppDriverFactoryConfiguration()
        {
            SUT = new AppDriverFactory();
        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        public void Creating_App_Driver_With_No_Config_Throws_Exception()
        {
            Assert.Throws<MissingConfigurationException>(() => SUT.Create());
        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        public void Create_App_Driver_With_Explicit_URL()
        {
            AppDriver result = SUT.Driving(DESIRED_URL).Create();

            Assert.NotNull(result.BaseUrl);
            Assert.Equal<string>(result.BaseUrl.ToString(), DESIRED_URL);
        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        public void Creates_AppDriver_specifying_baseURL_fisrt_and_then_Browser()
        {
            //set base url first, browser second
            AppDriver result = SUT.Driving(DESIRED_URL).Using<PhantomJSDriver>().Create();

            Assert.NotNull(result.WebDriver);
            Assert.Same(DESIRED_URL, result.BaseUrl.ToString());
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
            Assert.Same(DESIRED_URL, result.BaseUrl.ToString());
        }
    }
}
