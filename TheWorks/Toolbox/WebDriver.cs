using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorks.Toolbox
{
    public class WebDriver
    {
        private const string startUrl = "https://www.omada.net";
        private TimeSpan waitTimeout = TimeSpan.FromSeconds(60);
        IWebDriver _webDriver;

        public WebDriver(WebDriverType type)
        {
            switch(type)
            {
                case WebDriverType.Chrome:
                    _webDriver = new ChromeDriver();
                    break;
                case WebDriverType.Firefox:
                    _webDriver = new FirefoxDriver();
                    break;
            }

            StartDriving();
        }

        private void StartDriving()
        {
            _webDriver.Manage().Window.Maximize();
            _webDriver.Navigate().GoToUrl(startUrl);
            WaitForPageLoaded();
            CloseCookieInfo();
        }

        public void WaitForPageLoaded()
        {
            WebDriverWait wait = new WebDriverWait(_webDriver, waitTimeout);
            IJavaScriptExecutor js = (IJavaScriptExecutor)_webDriver;
            wait.Until(d => js.ExecuteScript($"return document.readyState").ToString().ToUpper() == "COMPLETE");
        }

        public void SubmitForm(IWebElement form)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_webDriver;
            js.ExecuteScript("arguments[0].submit()", form);
        }

        public IWebElement FindElement(By by) =>
            _webDriver.FindElement(by);

        public List<IWebElement> FindElements(By by) =>
            _webDriver.FindElements(by).ToList();

        public void SwitchToFrame(IWebElement frame)
        {
            _webDriver.SwitchTo().Frame(frame);
        }

        public void SwitchToDefault()
        {
            _webDriver.SwitchTo().DefaultContent();
        }

        public void MoveToElement(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_webDriver;
            js.ExecuteScript("arguments[0].scrollIntoView(true)", element);
            js.ExecuteScript("var re = arguments[0].getBoundingClientRect(); " +
                "if(re.top < (window.pageYOffset - 200)) {window.scrollBy(0, -200)};",
                element);
        }

        public void Reload()
        {
            _webDriver.Navigate().Refresh();
            WaitForPageLoaded();
        }

        public void Dispose()
        {
            _webDriver.Quit();
        }

        private void CloseCookieInfo()
        {
            var container = _webDriver.FindElement(By.ClassName("cookiebar__container"));
            if (container != null)
                container.FindElement(By.ClassName("cookiebar__button")).Click();
        }
    }
}
