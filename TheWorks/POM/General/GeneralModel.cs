using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorks.Toolbox;

namespace TheWorks.POM
{
    public class GeneralModel
    {
        private IWebElement _languageMenu;

        protected WebDriver _webDriver;
        private const string search = "header__search";
        private const string requestFormSrc = "go.pardot.com";

        public enum Languages
        {
            EN,
            DE,
            DK
        }

        public GeneralModel(WebDriver webDriver)
        {
            _webDriver = webDriver;
            FillWebElements();
        }

        public SearchResults Search(string stringToSearch)
        {
            var input = _webDriver.FindElement(By.ClassName(search))
                .FindElement(By.TagName("input"));
            input.Clear();
            input.SendKeys(stringToSearch);
            input.SendKeys(Keys.Return);
            _webDriver.WaitForPageLoaded();
            return new SearchResults(_webDriver);
        }

        public GeneralModel SwitchLanguage(Languages language)
        {
            if (language == GetActiveLanguage())
                return this;

            _languageMenu.FindElements(By.ClassName("header__menulink--function-nav"))
                .First(e => e.Text == language.ToString()).Click();

            _webDriver.WaitForPageLoaded();

            return this;
        }

        public bool HasButton(string buttonText)
        {
            try
            {
                _webDriver.FindElement(By.PartialLinkText(buttonText));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public RequestForm GetRequestForm()
        {
            return new RequestForm(_webDriver);
        }

        public GeneralModel ClickButton(string buttonText)
        {
            var button = _webDriver.FindElement(By.PartialLinkText(buttonText));
            _webDriver.MoveToElement(button);
            button.Click();
            _webDriver.WaitForPageLoaded();
            return this;
        }

        public Languages ActiveLanguage
        {
            get
            {
                return GetActiveLanguage();
            }
        }


        public bool PageContainsWord(string wordToSearch)
        {
            return _webDriver.FindElements(By.TagName("p")).Any(e => e.Text.Contains(wordToSearch))
                || _webDriver.FindElements(By.TagName("a")).Any(e => e.Text.Contains(wordToSearch));
        }

        public bool PageContainsRequestForm()
        {
            return _webDriver.FindElements(By.TagName("iframe"))
                .Any(f => f.GetAttribute("src").Contains(requestFormSrc) &&
                    f.GetAttribute("height") == "500");
        }

        private Languages GetActiveLanguage()
        {
            return (Languages)Enum.Parse(typeof(Languages),
                _languageMenu.FindElement(By.ClassName("is-in-path")).FindElement(By.TagName("a")).Text);
        }

        private void FillWebElements()
        {
            _languageMenu = _webDriver.FindElement(By.ClassName("header__function-nav--left"));
        }

        protected void SwitchToRequestForm()
        {
            var requestForm = _webDriver.FindElements(By.TagName("iframe"))
                .First(f => f.GetAttribute("src").Contains(requestFormSrc));
            _webDriver.MoveToElement(requestForm);
            _webDriver.SwitchToFrame(requestForm);
        }

        protected void SwitchBack()
        {
            _webDriver.SwitchToDefault();
        }
    }
}
