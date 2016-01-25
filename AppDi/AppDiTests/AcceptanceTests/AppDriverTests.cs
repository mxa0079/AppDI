using AppDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppDiTests.AppDriverTests
{
    public class AppDriverTests
    {
        public class BingHomePageObject : PageObject
        {
            [FindsBy(How = How.Id, Using = "sb_form_q")]
            public IWebElement SearchBox { get; set; }

            [FindsBy(How = How.Id, Using = "sb_form_go")]
            public IWebElement SearchButton { get; set; }

            public BingHomePageObject Load()
            {
                this.WebDriver.Navigate().GoToUrl(this.Url.ToString());
                return this;
            }

            public BingHomePageObject Search(string term)
            {
                this.SearchBox.SendKeys(term);
                this.SearchButton.Click();
                return this;
            }
        }


        //Simple case using the framework end to end
        //Requires internet access to pass
        [Fact]
        [Trait("SUT", "AppDriver")]
        [Trait("Type", "UAT")]
        public void PerformSimpleBingUITest()
        {
            dynamic BingDriver = AppDriver.Factory()
                                          .Driving("http://www.bing.com")
                                          .Using<PhantomJSDriver>()
                                          .Register<BingHomePageObject>()
                                          .Create();

            BingDriver.BingHome.Load().Search("Denali");

            var searchResultsUrl = BingDriver.WebDriver.Value.Url.ToString();

            BingDriver.WebDriver.Value.Quit();

            Assert.Contains("search?q=Denali", searchResultsUrl);
        }
        
    }
}
