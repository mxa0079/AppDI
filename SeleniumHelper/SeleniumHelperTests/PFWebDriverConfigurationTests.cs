using SeleniumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SeleniumHelperTests
{
    /// <summary>
    /// Holds all the tests related to WebDriver configuration
    /// </summary>
    public class PFWebDriverConfigurationTests
    {
        PageObject SUT;

        public PFWebDriverConfigurationTests()
        {
            SUT = PageObjectFactory.Create<ConcretePageObject>();
        }

        [Fact]
        [Trait("Concern", "WebDriver")]
        public void Creates_PageObject_With_WebDriver()
        {
            //Shoul the driver be a lazy property?
            Assert.NotNull(SUT.WebDriver);
        }

        //[Fact]
        //[Trait("Concern", "WebDriver")]
        //public void Multiple_PageObjects_Should_Reuse_Same_Browser_Instance()
        //{
        //    var AdditionalPageObject = PageObjectFactory.Create<ConcretePageObject>();

        //    Assert.NotSame(SUT, AdditionalPageObject);
        //    //Factory should create additional page objects using the same driver
        //    Assert.Same(SUT.WebDriver, AdditionalPageObject.WebDriver);
        //}
    }
}
