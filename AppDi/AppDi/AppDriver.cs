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

        public AppDriver(Uri baseUrl, Lazy<IWebDriver> webDriver, Dictionary<string, Type> PageObjectmembers)
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