namespace AppDiTests.AppDriverFabricTests
{
    using AppDi;
    using System.Dynamic;
    using Xunit;

    public class AppDriverFactory_PageObjectRegistration
    {
        private AppDriverFactory SUT;

        public AppDriverFactory_PageObjectRegistration()
        {
            SUT = new AppDriverFactory();
        }

        [Fact]
        [Trait("SUT", "AppDriverFactory")]
        public void Of_an_object_not_derived_from_PageObject_Throws()
        {
            const string JSON_CONFIG_URL = @"http://www.google.com/";

            dynamic driver = SUT.Register<ConcretePageObject>().Create();

            Assert.Equal(JSON_CONFIG_URL, driver.Concrete.Url.ToString());
        }

    }
}
