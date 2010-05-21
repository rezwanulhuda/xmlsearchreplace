using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib.Validator;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest.ValidatorTest
{
    /// <summary>
    /// Summary description for EnsureEqualSearchReplaceStringValidatorTest
    /// </summary>
    [TestClass]
    public class EnsureEqualSearchReplaceStringValidatorTest
    {
        [TestMethod]
        public void IsValid_L_Missing_RS_Present_ShouldReturnTrue()
        {
            string commandLine = "/S=hello /R=world";
            EnsureEqualSearchReplaceStringValidator validator = new EnsureEqualSearchReplaceStringValidator();
            Assert.IsTrue(validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));

        }

        [TestMethod]
        public void IsValid_LS_Present_R_Missing_ShouldReturnTrue()
        {
            string commandLine = "/S=hello /L";
            EnsureEqualSearchReplaceStringValidator validator = new EnsureEqualSearchReplaceStringValidator();
            Assert.IsTrue(validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));

        }

        [TestMethod]
        public void IsValid_P_PresentWithAllSearchReplaceStrings_RSL_Missing_ShouldReturnTrue()
        {
            string paramFile = TestHelper.CreateParameterFile(new String[] { "/R=abc /S=bbc", "/R=1 /S=2" });
            string commandLine = String.Format("/P={0}", paramFile);
            EnsureEqualSearchReplaceStringValidator validator = new EnsureEqualSearchReplaceStringValidator();
            Assert.IsTrue(validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));

            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod]
        public void IsValid_P_PresentWithSomeReplaceStringMissing_RSL_Missing_ShouldReturnFalse()
        {
            string paramFile = TestHelper.CreateParameterFile(new String[] { "/R=abc /S=bbc", "/S=2" });
            string commandLine = String.Format("/P={0}", paramFile);
            EnsureEqualSearchReplaceStringValidator validator = new EnsureEqualSearchReplaceStringValidator();
            Assert.IsFalse(validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));

            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod]
        public void IsValid_PL_PresentWithAllReplaceStringMissing_RSL_Missing_ShouldReturnTrue()
        {
            string paramFile = TestHelper.CreateParameterFile(new String[] { "/S=bbc", "/S=2" });
            string commandLine = String.Format("/P={0} /L", paramFile);
            EnsureEqualSearchReplaceStringValidator validator = new EnsureEqualSearchReplaceStringValidator();
            Assert.IsTrue(validator.IsValid(TestHelper.GetCommandLineParameters(commandLine)));
            TestHelper.DeleteLastParameterFile();
        }    
    }
}
