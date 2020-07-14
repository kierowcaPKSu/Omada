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
    public class RequestFormTests : TestBase
    {
        private RequestForm _requestForm;

        public RequestFormTests(WebDriverType webDriverType) : base(webDriverType) { }

        [Test]
        public void CheckRequestForm()
        {
            GoToRequestForm();
            CheckEmailValidation();
            CheckForFieldsAtATime(1);
            CheckForFieldsAtATime(2);
            CheckForFieldsAtATime(3);
            CheckForFieldsAtATime(4);
            CheckForFieldsAtATime(5);
            CheckForFieldsAtATime(6);
            CheckPrivacyPolicy();
        }

        private void GoToRequestForm()
        {
            SearchResults searchResults = new SearchResults(WebDriver);
            searchResults = searchResults.Search("e-book");
            GeneralModel page = searchResults.ChooseResultByIndex(Utils.GetRandomIndexes(1, searchResults.ResultCount)[0]);
            if (!page.PageContainsRequestForm())
                GoToRequestForm();
            _requestForm = page.GetRequestForm();
        }

        private void CheckForFieldsAtATime(int fieldsAtATime)
        {
            var combinations = Utils.GetCombinations(_requestForm.PhoneRequired, fieldsAtATime
                );
            foreach(var combination in combinations)
            {
                _requestForm.FillByFieldNumbers(combination.ToString());
                _requestForm.AcceptPrivacyPolicy();
                _requestForm.Submit();
                Assert.IsTrue(_requestForm.FormHasErrors, "Form didn't have any errors though not all required fields were filled");
                _requestForm.ClearAll();
            }
        }

        private void CheckEmailValidation()
        {
            if (_requestForm.RequiresBusinessEmail)
            {
                _requestForm.WithEmail("test@gmail.com");
                _requestForm.Submit();
                Assert.IsTrue(_requestForm.EmailHasErrors, "Email field had no errors for an address from a popular domain");
                _requestForm.ClearAll();
            }

            _requestForm.WithEmail("test");
            _requestForm.Submit();
            Assert.IsTrue(_requestForm.EmailHasErrors, "Email field had no errors for an incomplete address");
            _requestForm.ClearAll();

            _requestForm.WithEmail("test@company");
            _requestForm.Submit();
            Assert.IsTrue(_requestForm.EmailHasErrors, "Email field had no errors for an incomplete address");
            _requestForm.ClearAll();

            _requestForm.WithEmail("test@business.com");
            _requestForm.Submit();
            Assert.IsFalse(_requestForm.EmailHasErrors, "Email field had errors for a proper email");
            _requestForm.ClearAll();
        }

        private void CheckPrivacyPolicy()
        {
            _requestForm.FillByFieldNumbers("12345678");
            _requestForm.Submit();
            Assert.IsTrue(_requestForm.FormHasErrors, "The form didn't have errors despite privacy policy not being accepted");
        }
    }
}
