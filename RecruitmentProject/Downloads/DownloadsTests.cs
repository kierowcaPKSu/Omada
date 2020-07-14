using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorks.POM;
using TheWorks.Toolbox;

namespace RecruitmentProject
{
    public class DownloadsTests : TestBase
    {
        private const int formCheckRepetitions = 3;
        private const string largeFileSearchWord = "e-book";
        private const string ebookButton = "Download";

        public DownloadsTests(WebDriverType webDriverType) : base(webDriverType) { }

        [Test]
        public void CheckIfRequestFormIsRequired()
        {
            SearchResults searchResults = new SearchResults(WebDriver);
            searchResults = searchResults.Search(largeFileSearchWord);
            int[] entriesToCheck = Utils.GetRandomIndexes(3, searchResults.ResultCount);
            foreach (int index in entriesToCheck)
            {
                CheckOneResult(index);
            }
        }

        public void CheckOneResult(int index)
        {
            SearchResults searchResults = new SearchResults(WebDriver);
            searchResults = searchResults.Search(largeFileSearchWord);
            var page = searchResults.ChooseResultByIndex(index);
            if (!page.HasButton(ebookButton))
                CheckOneResult(index + 1);
            page.ClickButton(ebookButton);
            Assert.IsTrue(page.PageContainsRequestForm(), "The result page didn't appear to contain a request form");
        }
    }
}
