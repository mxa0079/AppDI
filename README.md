![Build Badge Placeholder](https://mavil.visualstudio.com/DefaultCollection/_apis/public/build/definitions/19319cdc-2a49-457c-bb5a-3f377d03af28/6/badge "Build Status")

# AppDi Description

Write maintainable Selenium tests, fast.

AppDi is a .NET library designed to:

* Eliminate boiler plate code
* Encourage maintainable tests
* Provide transparent reporting

##SampleCode

AppDi allows you to write a test like this:

```csharp
        //Simple case using the framework end to end
        [Fact]
        public void PerformBingSearch()
        {
            //Create a driver to interact with bing.com
            dynamic BingDriver = AppDriver.Factory()
                                          .Using<PhantomJSDriver>()
                                          .Driving("http://www.bing.com")
                                          .Register<BingHomePageObject>()
                                          .Create();
            
            //BingHome property (pageobject) was dinamycally registered with our driver
            BingDriver.BingHome.Load().Search("Denali");
        }
```

Given that you have a PageObject like this:

```csharp
        //The parent PageObject is provided by the AppDi framework
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
```
Highlights from this example:

* AppDi allows you to start using Selenium's [PageObject Model] (https://code.google.com/p/selenium/wiki/PageObjects) without any boiler plate code.
* You are able to dinamically register PageObjects with your App Driver object at creation. Removing the need for classes whose only purpose is to expose a set of properties containing page objects.


