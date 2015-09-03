using SeleniumHelper;
using Xunit;

namespace SeleniumHelperTests
{
    public class PageFactoryTests
    {
        public class ConcretePageObject : PageObject
        {

        }

        [Fact]
        public void Creates_PageObject_With_Explicit_Base_URL()
        {
            var BingHomePage = PageObjectFactory.Create<ConcretePageObject>("http://www.bing.com");

            Assert.Equal<string>("http://www.bing.com/", BingHomePage.Url.ToString());
        }

        [Fact]
        public void Creates_PageObject_With_Config_Based_Url()
        {
            //Since no URL is provided, PageFactory will try to read it from config file
            var GoogleHomePage = PageObjectFactory.Create<ConcretePageObject>();

            Assert.Equal<string>("http://www.google.com/", GoogleHomePage.Url.ToString());
        }

        [Fact]
        public void Creates_PageObject_With_WebDriver()
        {
            var SUT = PageObjectFactory.Create<ConcretePageObject>();

            //Shoul the driver be a lazy property?
            Assert.NotNull(SUT.WebDriver);
        }
    }
}
