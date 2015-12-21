namespace AppDiTests.AppDriverFabricTests
{
    using AppDi;
    using OpenQA.Selenium.PhantomJS;
    using Xunit;
    using VanillaPageObjects;

    public class AppDriverFactory_Dynamic_PageObjectRegistration
    {
        private AppDriverFactory SUT;

        public AppDriverFactory_Dynamic_PageObjectRegistration()
        {
            SUT = AppDriver.Factory();
        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        [Trait("Area", "Dynamic Registration")]
        public void Works_Reistering_A_Single_Page_At_A_time()
        {
            //This is the URL configured on appdi.config.json and hence it should be the URL value of our Concrete page object
            //See test "Creates_AppDriver_taking_defaults_from_JSON_config()" for details
            const string EXPECTED_CONFIG_URL = @"http://www.google.com/";

            //Register a Page Object with our app driver
            dynamic driver = SUT.Using<PhantomJSDriver>().Register<ConcretePageObject>().Create();

            var pageObjectUrl = driver.Concrete.Url.ToString();

            driver.WebDriver.Value.Quit();

            //Ensure we have access to the dynamic page object we just registered
            //And that it was created as expected
            Assert.Equal(EXPECTED_CONFIG_URL, pageObjectUrl);

        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        [Trait("Area", "Dynamic Registration")]
        public void Works_Reistering_A_NamseSpace()
        {
            //This is the URL configured on appdi.config.json and hence it should be the URL value of our Concrete page object
            //See test "Creates_AppDriver_taking_defaults_from_JSON_config()" for details
            const string EXPECTED_CONFIG_URL = @"http://www.google.com/";

            //Register a Page Object with our app driver
            dynamic driver = SUT.Using<PhantomJSDriver>().Register("AppDiTests.VanillaPageObjects").Create();

            var concretePageObjectUrl = driver.Concrete.Url.ToString();
            var vanillaPageObjectUrl = driver.Vanilla.Url.ToString();

            driver.WebDriver.Value.Quit();

            //Ensure we have access to the dynamic page object we just registered
            //And that it was created as expected
            Assert.Equal(EXPECTED_CONFIG_URL, concretePageObjectUrl);
            Assert.Equal(EXPECTED_CONFIG_URL, vanillaPageObjectUrl);

        }

    }
}
