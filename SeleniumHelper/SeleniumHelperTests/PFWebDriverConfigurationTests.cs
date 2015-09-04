using SeleniumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace SeleniumHelperTests
{
    /// <summary>
    /// Holds all the tests related to WebDriver configuration
    /// </summary>
    public class PFWebDriverConfigurationTests : IDisposable
    {
        PageObject SUT;

        public PFWebDriverConfigurationTests()
        {
            SUT = PageObjectFactory.Create<ConcretePageObject>();
        }

        public void Dispose()
        {
            SUT.WebDriver.Quit();
        }

        [Fact]
        [Trait("Concern", "WebDriver")]
        public void Creates_PageObject_With_WebDriver()
        {
            //Shoul the driver be a lazy property?
            Assert.NotNull(SUT.WebDriver);
        }

        [Fact]
        [Trait("Concern", "WebDriver")]
        public void Multiple_PageObjects_Should_Reuse_Same_Browser_Instance()
        {
            var AdditionalPageObject = PageObjectFactory.Create<ConcretePageObject>();

            Assert.NotSame(SUT, AdditionalPageObject);

            try
            {
                //Factory should create additional page objects using the same driver
                Assert.Same(SUT.WebDriver, AdditionalPageObject.WebDriver);
            }
            //Catch assertion failure so we can properly close additional browser.
            catch (SameException se)
            {
                AdditionalPageObject.WebDriver.Quit();
                throw;
            }
        }

    }
}
