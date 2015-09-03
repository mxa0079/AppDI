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
        [Fact]
        public void Creates_PageObject_With_WebDriver()
        {
            var SUT = PageObjectFactory.Create<ConcretePageObject>();

            //Shoul the driver be a lazy property?
            Assert.NotNull(SUT.WebDriver);
        }
    }
}
