namespace AppDiTests.AppDriverFabricTests
{
    using AppDi;
    using OpenQA.Selenium.PhantomJS;
    using System.Diagnostics;
    using System.Dynamic;
    using Xunit;

    public class AppDriverFactory_Dynamic_PageObjectRegistration
    {
        private AppDriverFactory SUT;

        public AppDriverFactory_Dynamic_PageObjectRegistration()
        {
            SUT = AppDriver.Factory();
        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        public void Works_Registering_A_Single_Page_At_A_time()
        {
            //This is the URL configured on appdi.config.json and hence it should be the URL value of our Concrete page object
            //See test "Creates_AppDriver_taking_defaults_from_JSON_config()" for details
            const string JSON_CONFIG_URL = @"http://www.google.com/";

            //Register a Page Object with our app driver
            dynamic driver = SUT.Using<PhantomJSDriver>().Register<ConcretePageObject>().Create();

            var pageObjectUrl = driver.Concrete.Url.ToString();

            driver.WebDriver.Value.Quit();

            //Ensure we have access to the dynamic page object we just registered
            //And that it was created as expected
            Assert.Equal(JSON_CONFIG_URL, pageObjectUrl);

        }

    }
}
