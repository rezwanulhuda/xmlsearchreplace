﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib;

namespace XmlSnRTest.XmlSearchReplaceConsoleLibTest
{    
    [TestClass]
    public class SParameterValidatorTest
    {
        [TestMethod]
        public void Validate_WhenPMissing_ShouldNotValidate()
        {
            ApplicationParameterWithValueCollection coll = TestHelper.GetParameters("") as ApplicationParameterWithValueCollection;

            Assert.IsFalse(SParamValidator.Validate(coll));
        }

        [TestMethod]
        public void Validate_WhenPNotMissing_ShouldValidate()
        {
            ApplicationParameterWithValueCollection coll = TestHelper.GetParameters("/P=abc") as ApplicationParameterWithValueCollection;

            Assert.IsTrue(SParamValidator.Validate(coll));
        }
    }

    public class SParamValidator
    {
        public static bool Validate(ApplicationParameterWithValueCollection parameters)
        {
            return parameters.Where(p => String.Compare(p.GetName(), "P", true) == 0).Count() > 0;

        }
    }

    [TestClass]
    public class RParameterValidatorTest
    {
        [TestMethod]
        public void Validate_WhenPandRMissing_ValidateReturnsFalse()
        {
            ApplicationParameterWithValueCollection coll = TestHelper.GetParameters("") as ApplicationParameterWithValueCollection;

            Assert.IsFalse(RParamValidator.Validate(coll));
        }

        //[TestMethod]
        //public void Validate_When_R_MissingAnd_P_NotMissing_ValidateReturnsTrue()
        //{
        //    ApplicationParameterWithValueCollection coll = TestHelper.GetParameters("/P=abc") as ApplicationParameterWithValueCollection;

        //    Assert.IsTrue(RParamValidator.Validate(coll));
        //}

        [TestMethod]
        public void Validate_WhenRMissingAndLNotMissing_ValidateReturnsTrue()
        {
            ApplicationParameterWithValueCollection coll = TestHelper.GetParameters("/L=abc") as ApplicationParameterWithValueCollection;

            Assert.IsTrue(RParamValidator.Validate(coll));
        }

        [TestMethod]
        public void Validate_When_RL_MissingAnd_P_NotMissingAndParamFileDoesNotContain_R_ValidateReturnsFalse()
        {
            string paramFile = TestHelper.CreateParameterFile(new String[] { "", ""});
            ApplicationParameterWithValueCollection coll = TestHelper.GetParameters(String.Format("/P={0}", paramFile)) as ApplicationParameterWithValueCollection;
            Assert.IsFalse(RParamValidator.Validate(coll));
            TestHelper.DeleteLastParameterFile();
        }

        [TestMethod]
        public void Validate_When_RL_MissingAnd_P_NotMissingAndParamFileContains_R_ValidateReturnsFalse()
        {
            string paramFile = TestHelper.CreateParameterFile(new String[] { "/R=abc", "" });
            ApplicationParameterWithValueCollection coll = TestHelper.GetParameters(String.Format("/P={0}", paramFile)) as ApplicationParameterWithValueCollection;
            Assert.IsTrue(RParamValidator.Validate(coll));
            TestHelper.DeleteLastParameterFile();
        }
    }

    public class RParamValidator
    {
        public static bool Validate(ApplicationParameterWithValueCollection parameters)
        {
            List<ApplicationParameterWithValue> values = parameters.FindAll(p => String.Compare(p.GetName(), "P", true) == 0);

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
