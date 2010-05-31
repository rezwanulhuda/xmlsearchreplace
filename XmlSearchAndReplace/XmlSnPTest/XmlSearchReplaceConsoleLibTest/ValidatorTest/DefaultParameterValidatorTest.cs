using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib.Validator;
using XmlSearchReplaceConsoleLib;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest.ValidatorTest
{    
    [TestClass]
    public class DefaultParameterValidatorTest
    {
        private void SetupAndAssert(string expectedMessage, string commandLine)
        {
            DefaultParameterValidator validator = new DefaultParameterValidator();

            try
            {
                validator.CheckParameters(TestHelper.GetCommandLineParameters(commandLine));
            }
            catch (InvalidArgumentOptionException e)
            {
                Assert.AreEqual(expectedMessage, e.Message, "Right exception, wrong message");
                return;
            }
            Assert.Fail("No exception thrown");
        }
        
        [TestMethod]
        public void CheckParameters_EmptyString_ThrowsInvalidArgumentOptionException()
        {            
            SetupAndAssert(@"Following parameters are either missing or does not contain value: /O, /F", String.Empty);
        }

        [TestMethod]
        public void CheckParameters_OF_MissingValue_ThrowsInvalidArgumentOptionException()
        {
            SetupAndAssert(@"Following parameters are either missing or does not contain value: /O, /F", "/F /O");
        }

        [TestMethod]
        public void CheckParameters_UnsupportedParams_ThrowsInvalidArgumentOptionException()
        {
            SetupAndAssert(@"Application only supports the following parameters: /S, /R, /O, /F, /C, /I, /W, /D, /L, /P", "/A");
        }

        [TestMethod]
        public void CheckParameters_MissingS_ThrowsInvalidArgumentOptionException()
        {
            SetupAndAssert(@"Missing search string (/S) in command line", "/O=abc /F=abc");
        }

        [TestMethod, Ignore]
        public void CheckParameters_S_But_No_R_ThrowsInvalidArgumentOptionException()
        {
            SetupAndAssert(@"", "/O=abc /F=abc /S=abc");
        }

        [TestMethod]
        public void CheckParameters_P_Contains_S_But_No_R_ThrowsInvalidArgumentOptionException()
        {
            SetupAndAssert(@"Number of search and replace string must be equal. Check parameter file contains search and replace string in all of the lines.", String.Format("/O=abc /F=abc /P={0}", TestHelper.CreateParameterFile(new string[] { "/S" })));
            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod]
        public void CheckParameters_P_Contains_But_No_S_ThrowsInvalidArgumentOptionException()
        {
            SetupAndAssert(@"Search string missing in param file", String.Format("/O=abc /F=abc /P={0}", TestHelper.CreateParameterFile(new string[] { "/R=abc" })));
            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod, Ignore]        
        public void CheckParameters_Contains_FORS_ButRContainsSpace_ThrowsInvalidArugmentOptionException()
        {
            SetupAndAssert("One or more replace string contains invalid element or attribute names. When replace location contains element/attribute name, replace strings must be valid names suitable for element/attribute names", "/F=abc /O=en,an /R=je lo /S=ok");
        }

        [TestMethod]
        public void CheckParameters_Contains_FORS_ButODoesNotContain_ENEV_ReturnsOK()
        {
            DefaultParameterValidator validator = new DefaultParameterValidator();
            validator.CheckParameters(TestHelper.GetCommandLineParameters("/F=abc /O=av,ev /R=je lo /S=ok"));
        }

        [TestMethod]
        public void CheckParameters_Contains_SRPF_ValidatesOk()
        {
            DefaultParameterValidator validator = new DefaultParameterValidator();
            validator.CheckParameters(TestHelper.GetCommandLineParameters("/F=abc /O=abc /R=jelo /S=ok"));
        }



        


    }
}
