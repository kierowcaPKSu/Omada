using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorks.Toolbox;

namespace TheWorks.POM
{
    public class SearchResults : GeneralModel
    {
        private List<SingleResult> _results;

        public SearchResults(WebDriver webDriver) : base(webDriver)
        {
            LoadResults();
        }

        public void LoadResults()
        {
            var results = _webDriver.FindElements(By.ClassName("search-results__item"));
            _results = new List<SingleResult>();
            results.ForEach(r => _results.Add(new SingleResult(r)));
        }

        public GeneralModel ChooseResultByIndex(int index)
        {
            _results[index].Click();
            _webDriver.WaitForPageLoaded();
            return new GeneralModel(_webDriver);
        }

        public GeneralModel ChooseResultByTitle(string titleFragment)
        {
            _results.First(r => r.Header.Contains(titleFragment)).Click();
            _webDriver.WaitForPageLoaded();
            return new GeneralModel(_webDriver);
        }

        public GeneralModel ChooseResultByText(string textFragment)
        {
            _results.First(r => r.Info.Contains(textFragment)).Click();
            _webDriver.WaitForPageLoaded();
            return new GeneralModel(_webDriver);
        }

        public int ResultCount =>
            _results.Count();

        private class SingleResult
        {
            IWebElement _result;

            public SingleResult(IWebElement result)
            {
                _result = result;
            }

            public string Header
            {
                get
                {
                    return _result.FindElement(By.TagName("a")).Text;
                }
            }

            public string Info
            {
                get
                {
                    return _result.FindElement(By.TagName("p")).Text;
                }
            }

            public void Click()
            {
                var toCLick = _result.FindElement(By.TagName("a"));
                toCLick.Click();
            }
        }
    }
}
