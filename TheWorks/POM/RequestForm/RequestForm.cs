using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorks.Toolbox;

namespace TheWorks.POM
{
    public class RequestForm : GeneralModel
    {
        private enum Fields
        {
            first_name = 1,
            last_name = 2,
            company = 3,
            job_title = 4,
            email = 5,
            phone = 6,
            sf_Number_of_Employees = 7,
            country = 8
        }

        public RequestForm(WebDriver webDriver) : base(webDriver)
        {
        }

        public RequestForm WithFirstName(string firstName)
        {
            SetField(Fields.first_name, firstName);
            return this;
        }

        public RequestForm WithLastName(string lastName)
        {
            SetField(Fields.last_name, lastName);
            return this;
        }

        public RequestForm WithCompany(string company)
        {
            SetField(Fields.company, company);
            return this;
        }

        public RequestForm WithJobTitle(string jobTitle)
        {
            SetField(Fields.job_title, jobTitle);
            return this;
        }

        public RequestForm WithEmail(string email)
        {
            SetField(Fields.email, email);
            return this;
        }

        public RequestForm WithPhone(string phone)
        {
            SetField(Fields.phone, phone);
            return this;
        }

        public RequestForm WithCountry(string country)
        {
            SetField(Fields.country, country);
            return this;
        }

        public RequestForm WithNumberOfEmployees(int numberOfEmployees)
        {
            SetField(Fields.sf_Number_of_Employees, ParseNumberOfEmployees(numberOfEmployees));
            return this;
        }

        public RequestForm AcceptPrivacyPolicy()
        {
            SwitchToRequestForm();
            _webDriver.FindElement(By.ClassName("Lead_Marketing_Permission"))
                .FindElement(By.TagName("input"))
                .Click();
            SwitchBack();
            return this;
        }

        public RequestForm Submit()
        {
            SwitchToRequestForm();
            _webDriver.SubmitForm(_webDriver.FindElement(By.Id("pardot-form")));
            _webDriver.WaitForPageLoaded();
            SwitchBack();
            return this;
        }

        public RequestForm ClearAll()
        {
            _webDriver.Reload();
            return this;
        }

        public RequestForm FillByFieldNumbers(string numbers)
        {
            var singles = numbers.Select(x => int.Parse(x.ToString()));
            singles = RemoveNotPresent(singles.ToList());
            foreach (var single in singles)
            {
                var content = GetContentByField((Fields)single);
                SetField((Fields)single, content);
            }
            return this;
        }

        public bool PhoneRequired
        {
            get
            {
                SwitchToRequestForm();
                bool phoneRequired = _webDriver.FindElement(By.ClassName("phone"))
                .GetAttribute("class")
                .Contains("required");
                SwitchBack();
                return phoneRequired;
            }
            
        }

        public bool FormHasErrors
        {
            get
            {
                SwitchToRequestForm();
                bool hasErrors = _webDriver.FindElements(By.ClassName("error")).Count > 2;
                SwitchBack();
                return hasErrors;
            }
        }

        public bool EmailHasErrors
        {
            get
            {
                SwitchToRequestForm();
                bool hasErrors = _webDriver.FindElement(By.ClassName(Fields.email.ToString()))
                    .GetAttribute("class").Contains("error");
                SwitchBack();
                return hasErrors;
            }
        }

        public bool RequiresBusinessEmail
        {
            get
            {
                SwitchToRequestForm();
                bool hasErrors = _webDriver.FindElement(By.ClassName(Fields.email.ToString()))
                    .FindElement(By.TagName("label")).Text.Contains("business");
                SwitchBack();
                return hasErrors;
            }
        }

        private void SetField(Fields field, string stringToSet)
        {
            if ((int)field > 6)
                SetDropdown(field.ToString(), stringToSet);
            else
                WriteToTextField(field.ToString(), stringToSet);
        }

        private void WriteToTextField(string fieldClass, string toWrite)
        {
            SwitchToRequestForm();
            var box = _webDriver.FindElement(By.ClassName(fieldClass))
                .FindElement(By.TagName("input"));
            box.Clear();
            if(!string.IsNullOrEmpty(toWrite))
                box.SendKeys(toWrite);
            SwitchBack();
        }

        private void SetDropdown(string fieldClass, string textToSet)
        {
            SwitchToRequestForm();
            System.Threading.Thread.Sleep(500);
            var sf = _webDriver.FindElement(By.ClassName(fieldClass)).FindElement(By.TagName("select"));
            sf.Click();
            sf.FindElements(By.TagName("option")).First(o => o.Text.Contains(textToSet)).Click();
            SwitchBack();
        }

        private string ParseNumberOfEmployees(int number)
        {
            if (number > 10000)
                return "+10000";
            if (number > 5000)
                return "5000-10000";
            if (number > 1000)
                return "1000-5000";
            if (number > 500)
                return "500-1000";
            return "0-500";
        }

        private string GetContentByField(Fields field)
        {
            switch (field)
            {
                case Fields.sf_Number_of_Employees:
                    return ParseNumberOfEmployees(Utils.GetRandomIndexes(1, 20000)[0]);
                case Fields.country:
                    return "Poland";
                case Fields.email:
                    return Guid.NewGuid().ToString() + "@business.com";
                default:
                    return Guid.NewGuid().ToString();
            }
        }

        private List<int> RemoveNotPresent(List<int> singles)
        {
            List<int> resultList = new List<int>();
            foreach (var single in singles)
                if (FieldPresent(single))
                    resultList.Add(single);
            return resultList;
        }

        private bool FieldPresent(int fieldNumber)
        {
            SwitchToRequestForm();
            bool present = _webDriver.FindElements(By.ClassName(((Fields)fieldNumber).ToString())).Any();
            SwitchBack();
            return present;
        }
    }
}
