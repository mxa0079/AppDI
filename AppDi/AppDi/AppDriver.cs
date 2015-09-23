using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace AppDi
{
    public class AppDriver : DynamicObject
    {
        public Uri BaseUrl { get; }
        public Lazy<IWebDriver> WebDriver { get; }

        private Dictionary<string, Type> _pageObjects;

        /// <summary>
        /// The AppDriverFactory is responsible for creating an instance of an AppDriver
        /// </summary>
        /// <returns></returns>
        public static AppDriverFactory Factory()
        {
            return new AppDriverFactory((Uri baseUrl, Lazy<IWebDriver> webDriver, Dictionary<string, Type> PageObjectmembers) =>
            {
                return new AppDriver(baseUrl, webDriver, PageObjectmembers);
            });
        }

        /// <summary>
        /// AppDriver should only be instantiated through its factory
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="webDriver"></param>
        /// <param name="PageObjectmembers"></param>
        private AppDriver(Uri baseUrl, Lazy<IWebDriver> webDriver, Dictionary<string, Type> PageObjectmembers)
        {
            this.BaseUrl = baseUrl;
            this.WebDriver = webDriver;
            _pageObjects = new Dictionary<string, Type>(PageObjectmembers);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            Type pageType;
            bool isPageRegistered = _pageObjects.TryGetValue(binder.Name, out pageType);

            if (isPageRegistered)
            {
                var createdPage = (PageObject)Activator.CreateInstance(pageType);
                createdPage.Url = this.BaseUrl;
                result = createdPage;
                return isPageRegistered;
            }
            else
            {
                return base.TryGetMember(binder, out result);
            }

        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {

            return base.TrySetMember(binder, value);
        }
    }
}