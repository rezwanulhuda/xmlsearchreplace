using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib.Validator;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest.ValidatorTest
{
    /// <summary>
    /// Summary description for EnsureSearchParameterWithParamFileTest
    /// </summary>
    [TestClass]
    public class EnsureSearchParameterWithParamFileTest
    {

        [TestCleanup]
        public void Cleanup()
        {
            TestHelper.DeleteLastParameterFile();
        }
        
        private void SetupAndAssert(bool expectedResult, string[] paramFileContent)
        {
            string paramFile = TestHelper.CreateParameterFile(paramFileContent);
            string commandLine = String.Format("/P={0}", paramFile);
            EnsureSearchParameterWithParamFile validator = new EnsureSearchParameterWithParamFile();
            Assert.AreEqual(expectedResult, validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));            
        }
        
        [TestMethod]
        public void IsValid_With_P_Containing_S_InAllLines_ReturnsTrue()
        {
            SetupAndAssert(true, new String[] { "/S=abc", "/S=bbc" });
        }

        [TestMethod]
        public void IsValid_With_P_But_S_MissingInSomeLines_ReturnsFalse()
        {
            SetupAndAssert(false, new String[] { "/S=abc", "" });
        }

        [TestMethod]
        public void IsValid_Missing_P_ReturnsFalse()
        {
            EnsureSearchParameterWithParamFile validator = new EnsureSearchParameterWithParamFile();
            Assert.AreEqual(false, validator.IsValid(TestHelper.GetCommandLineParameters(String.Empty)));
        }
    }
}
