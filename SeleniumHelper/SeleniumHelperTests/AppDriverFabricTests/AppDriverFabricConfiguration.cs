using OpenQA.Selenium.PhantomJS;
using SeleniumHelper;
using System;
using Xunit;

namespace SeleniumHelperTests.AppDriverFabricTests
{
    public class AppDriverFabricConfiguration
    {
        private AppDriverFabric SUT;

        private readonly string DESIRED_URL = "http://www.bing.com/";


        public AppDriverFabricConfiguration()
        {
            SUT = new AppDriverFabric();
        }

        [Fact]
        [Trait("SUT", "AppDriverFabric")]
        public void Creating_App_Driver_With_No_Config_Throws_Exception()
        {
            Assert.Throws<MissingConfigurationException>(() => SUT.Create());
        }

        [Fact]
        [Trait("SUT", "AppDriverFabric")]
        public void Create_App_Driver_With_Explicit_URL()
        {
            AppDriver result = SUT.Driving(DESIRED_URL).Create();

            Assert.NotNull(result.BaseUrl);
            Assert.Equal<string>(result.BaseUrl.ToString(), DESIRED_URL);
        }

        [Fact]
        [Trait("SUT", "AppDriverFabric")]
        public void Creates_App_Driver_With_Correct_Base_Config()
        {
            AppDriverFabric sut = new AppDriverFabric();

            AppDriver result = sut.Driving(DESIRED_URL).Using<PhantomJSDriver>().Create();

            Assert.NotNull(result.WebDriver);
            
            //Initially, I had the assertion below. But this is redundant.
            //My code is not in charge of selecting the correct IWebDriver, I use generics for that
            //I should let the .net take care of testing that part
            //Great example of thinking through your assertions
            //Assert.IsType<Lazy<PhantomJSDriver>>(result.WebDriver);
            //It also resulted in some nasty cleanup code:
            //result.WebDriver.Value.Quit();
        }
    }
}
