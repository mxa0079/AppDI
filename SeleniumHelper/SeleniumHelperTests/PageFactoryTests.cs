using SeleniumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SeleniumHelperTests
{
    public class PageFactoryTests
    {
        public class BingHomePage : PageObject
        {

        }

        [Fact]
        public void Creates_SUTDriver_With_Correct_Base_URL()
        {
            var BingDriver = PageObjectFactory.Create<BingHomePage>("http://www.bing.com");

            Assert.Equal<string>("http://www.bing.com/", BingDriver.Url.ToString());
        }
    }
}
