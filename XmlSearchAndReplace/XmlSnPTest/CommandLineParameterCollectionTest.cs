using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlSearchReplaceConsoleLib;

namespace XmlSnRTest
{
    /// <summary>
    /// Summary description for CommandLineParameterCollectionTest
    /// </summary>
    [TestClass]
    public class CommandLineParameterCollectionTest
    {
        public CommandLineParameterCollectionTest()
        {            
        }

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
        public void GetUsage_WillReturnValidUsage()
        {
            string expectedCommandLine = @"/S=""search"" [/R]=""replace"" /O=en,ev,an,av /F=""C:\Files\*.xml"" [/C] [/I] [/W] [/D] [/L]";
            Assert.AreEqual(expectedCommandLine, ApplicationParameterCollection.GetUsage());
        }

        [TestMethod]
        public void GetHelpText_WillReturnHelpText()
        {
            string expectedHelpText = "S - " + HelpText.SearchString + Environment.NewLine;
            expectedHelpText += "R - " + HelpText.ReplaceString + " (optional)" + Environment.NewLine;
            expectedHelpText += "O - " + HelpText.Option + Environment.NewLine;            
            expectedHelpText += "F - " + HelpText.FileName + Environment.NewLine;            
            expectedHelpText += "C - " + HelpText.ContinueOnError + " (optional)" + Environment.NewLine;
            expectedHelpText += "I - " + HelpText.IgnoreCase + " (optional)" + Environment.NewLine;
            expectedHelpText += "W - " + HelpText.WholeWordOnly + " (optional)" + Environment.NewLine;
            expectedHelpText += "D - " + HelpText.RecurseSubDir + " (optional)" + Environment.NewLine;
            expectedHelpText += "L - " + HelpText.ReplaceSearchStringByLowerCase + " (optional)" + Environment.NewLine;
            expectedHelpText += HelpText.MoreInfo;

            Assert.AreEqual(expectedHelpText, ApplicationParameterCollection.GetHelpText());
        }
    }
}
