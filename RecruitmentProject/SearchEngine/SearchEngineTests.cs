using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using TheWorks.POM;
using TheWorks.Toolbox;

namespace RecruitmentProject
{
    public class SearchEngineTests : TestBase
    {
        private const int resultsToCheck = 5;

        public SearchEngineTests(WebDriverType webDriverType) : base(webDriverType) { }

        /// <summary>
        /// This test will fail in some cases during the Qualitative check part
        /// as not all pages that the search engine returns do indeed contain
        /// the word that is searched for. This is the case with Partner overview page
        /// </summary>
        [Test]
        public void CheckSearchEngine()
        {
            QuantitativeCheck("download");
            QuantitativeCheck("academy");
            QuantitativeCheck("register");
            QuantitativeCheck("security");
            QualitativeChecks("brochure");
        }

        private void QuantitativeCheck(string wordToSearch)
        {
            SearchResults searchResults = new SearchResults(WebDriver);
            searchResults = searchResults.Search(wordToSearch);
            int resultsNumber = searchResults.ResultCount;
            searchResults = searchResults.Search(wordToSearch);
            Assert.AreEqual(resultsNumber, searchResults.ResultCount,
                $"Second search for \"{wordToSearch}\" returned a different number of results");
        }

        private void QualitativeChecks(string wordToSearch)
        {
            SearchResults searchResults = new SearchResults(WebDriver);
            searchResults = searchResults.Search(wordToSearch);
            int resultAmount = resultsToCheck > searchResults.ResultCount
                ? searchResults.ResultCount
                : resultsToCheck;
            int[] resultNumbers = Utils.GetRandomIndexes(resultAmount, searchResults.ResultCount);
            foreach (var number in resultNumbers)
                Assert.IsTrue(OneQualitativeCheck(number, wordToSearch, searchResults), 
                    $"Result number {number} did not contain the word {wordToSearch}");
        }

        private bool OneQualitativeCheck(int resultNumber, string wordToCheck, SearchResults searchResults)
        {
            searchResults.Search(wordToCheck);
            var general = searchResults.ChooseResultByIndex(resultNumber);
            return general.PageContainsWord(wordToCheck);
        }

    }
}
