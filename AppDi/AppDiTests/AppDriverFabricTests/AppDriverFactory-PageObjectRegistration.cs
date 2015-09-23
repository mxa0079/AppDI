namespace AppDiTests.AppDriverFabricTests
{
    using AppDi;
    using System.Diagnostics;
    using System.Dynamic;
    using Xunit;

    public class AppDriverFactory_PageObjectRegistration
    {
        private AppDriverFactory SUT;

        public AppDriverFactory_PageObjectRegistration()
        {
            SUT = AppDriver.Factory();
        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        public void Using_Dynamic_AppDriver_Allows_PageObject_Registration()
        {
            const string JSON_CONFIG_URL = @"http://www.google.com/";

            dynamic driver = SUT.Register<ConcretePageObject>().Create();

            Assert.Equal(JSON_CONFIG_URL, driver.Concrete.Url.ToString());
        }

    }
}
