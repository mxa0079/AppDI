![Build Badge Placeholder](https://mavil.visualstudio.com/DefaultCollection/_apis/public/build/definitions/19319cdc-2a49-457c-bb5a-3f377d03af28/6/badge "Build Status")

# AppDi Description

Write maintainable Selenium tests, fast.

AppDi is a .NET library designed to:

* Eliminate boiler plate code
* Encourage maintainable tests
* Provide transparent reporting (upcoming release)

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
            //Convention for dynamic properties is to use 'PageObject' suffix, which is removed from actual property name
            //e.g. A class named BingHomePageObject will be registed with the AppDriver as BingHome
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

New feature in v 0.9.2-BETA.1:

###Namespace based dynamic PageObject registration

Given that you have referenced/loaded a namespace containing clases that derive from PageObject, such as:


```csharp
        namespace VanillaPageObjects
        {
                /// <summary>
                /// A vanilla helper class to enable unit testing.
                /// </summary>
                public class ConcretePageObject : PageObject
                {
                        
                }

                 /// <summary>
                /// A vanilla helper class to enable unit testing.
                /// </summary>
                public class VanillaPageObject : PageObject
                {

                }
        }
```

You can then register all PageObject under that namespace at once:

```csharp
        //Note that I am explicitly loading the namespace that I am about to register
        using VanillaPageObjects;
        
        public void Ignores_Classes_That_Do_Not_Have_PageObject_suffix()
        {
                //We are passing a string of the namespace containing the PageObjects we want to use. 
                //Only classes that directly derive from PageObject will be registered.
                dynamic driver = SUT.Using<PhantomJSDriver>().Register("VanillaPageObjects").Create();

                //The created AppDriver has two Page Objects registered (Concrete, and Vanilla)
                Assert.Equal(2, driver.PageObjectsCount);
        }
```
