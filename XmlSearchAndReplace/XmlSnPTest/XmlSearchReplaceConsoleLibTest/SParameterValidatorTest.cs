using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib;
using System.IO;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest
{    
    [TestClass]
    public class SParameterValidatorTest
    {
        [TestMethod]
        public void Validate_WhenPMissing_ShouldNotValidate()
        {
            CommandLineParameterWithValueCollection coll = TestHelper.GetCommandLineParameters("");

            Assert.IsFalse(SParamValidator.Validate(coll));
        }

        [TestMethod]
        public void Validate_WhenPNotMissing_ShouldValidate()
        {
            CommandLineParameterWithValueCollection coll = TestHelper.GetCommandLineParameters("/P=abc");

            Assert.IsTrue(SParamValidator.Validate(coll));
        }
    }

    public class SParamValidator
    {
        public static bool Validate(CommandLineParameterWithValueCollection parameters)
        {
            return parameters.Where(p => String.Compare(p.GetName(), "P", true) == 0).Count() > 0;

        }
    }

    [TestClass]
    public class RParameterValidatorTest
    {
        [TestCleanup]
        public void TestCleanup()
        {
            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod]
        public void Validate_WhenPandRMissing_ValidateReturnsFalse()
        {
            CommandLineParameterWithValueCollection coll = TestHelper.GetCommandLineParameters("");

            Assert.IsFalse(RParamValidator.Validate(coll));
        }        

        [TestMethod]
        public void Validate_WhenRMissingAndLNotMissing_ValidateReturnsTrue()
        {
            CommandLineParameterWithValueCollection coll = TestHelper.GetCommandLineParameters("/L=abc");

            Assert.IsTrue(RParamValidator.Validate(coll));
        }

        [TestMethod]
        public void Validate_When_RL_MissingAnd_P_NotMissingAndParamFileDoesNotContain_R_ValidateReturnsFalse()
        {
            string paramFile = TestHelper.CreateParameterFile(new String[] { "", ""});
            CommandLineParameterWithValueCollection coll = TestHelper.GetCommandLineParameters(String.Format("/P={0}", paramFile));
            Assert.IsFalse(RParamValidator.Validate(coll));            
        }

        [TestMethod]        
        public void Validate_When_RL_MissingAnd_P_NotMissingAndParamFileContains_R_ValidateReturnsFalse()
        {
            string paramFile = TestHelper.CreateParameterFile(new String[] { "/R=abc", "" });
            CommandLineParameterWithValueCollection coll = TestHelper.GetCommandLineParameters(String.Format("/P={0}", paramFile));
            Assert.IsTrue(RParamValidator.Validate(coll));
        }

        [TestMethod]        
        public void Validate_When_RL_MissingAnd_P_NotMissingAndParamFileContains_RP_ValidateThrowsException()
        {
            string paramFile = Path.GetTempFileName();
            File.WriteAllText(paramFile, String.Format(@"/R=abc /P={0}", paramFile));
            CommandLineParameterWithValueCollection coll = TestHelper.GetCommandLineParameters(String.Format("/P={0}", paramFile));
            try
            {
                Assert.IsFalse(RParamValidator.Validate(coll));
            }
            catch (InvalidArgumentOptionException e)
            {
                string eMsg = @"Parameter file only supports the following parameters:
S
R
";
                Assert.AreEqual(eMsg, e.Message);
                return;
            }

            Assert.Fail("Did not raise proper InvalidArgumentOptionException");

        }
    }

    public class RParamValidator
    {
        public static bool Validate(CommandLineParameterWithValueCollection parameters)
        {
            List<CommandLineParameterWithValue> values = parameters.FindAll(p => String.Compare(p.GetName(), "P", true) == 0);

            if (values.Count > 0)
            {
                string paramFilePath = values[0].GetValue();
                FileParamReader reader = new FileParamReader(paramFilePath);
                return reader.GetAllReplaceStrings().Count > 0;
            }
            

            return parameters.FindAll(p => String.Compare(p.GetName(), "L", true) == 0).Count() > 0;
        }
    }


    

    


    

    
}
