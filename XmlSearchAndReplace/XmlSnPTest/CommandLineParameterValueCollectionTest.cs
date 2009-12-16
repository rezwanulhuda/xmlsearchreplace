﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib;

namespace XmlSnRTest
{
    /// <summary>
    /// Summary description for CommandLineParameterValueCollectionTest
    /// </summary>
    [TestClass]
    public class CommandLineParameterValueCollectionTest
    {        

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        

        [TestMethod]
        public void CheckMissingParamsAreDetected()
        {
            CommandLineParameterCollection mandatoryParams = new CommandLineParameterCollection();
            mandatoryParams.Add(new CommandLineParameter("A", String.Empty, String.Empty, true));
            mandatoryParams.Add(new CommandLineParameter("B", String.Empty, String.Empty, false));

            CommandLineParameterValueCollection values = new CommandLineParameterValueCollection();
            values.Add(new CommandLineParameterValue(mandatoryParams[1], String.Empty));

            Assert.AreEqual(1, values.GetMissingMandatoryParams(mandatoryParams).Count);
            Assert.AreEqual("A", values.GetMissingMandatoryParams(mandatoryParams)[0].GetName());

        }
    }
}