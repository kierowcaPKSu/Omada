using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorks.Toolbox;

namespace RecruitmentProject
{
    [TestFixture(WebDriverType.Chrome)]
    [TestFixture(WebDriverType.Firefox)]
    public abstract class TestBase
    {
        public WebDriverType WebDriverType { get; private set; }

        public TestBase(WebDriverType webDriverType)
        {
            WebDriverType = webDriverType;
        }

        public WebDriver WebDriver { get; private set; }

        [TearDown]
        public void Cleanup()
        {
            WebDriver.Dispose();
        }

        [SetUp]
        public void TestSetUp()
        {
            WebDriver = new WebDriver(WebDriverType);
        }
    }
}
