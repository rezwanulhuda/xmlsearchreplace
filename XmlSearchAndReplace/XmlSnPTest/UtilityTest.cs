using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using XmlSearchReplaceLib;

namespace XmlSnRTest
{
    /// <summary>
    /// Summary description for UtilityTest
    /// </summary>
    [TestClass]
    public class UtilityTest
    {
        public UtilityTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Utility_GetBackupFileName()
        {
            Assert.AreEqual(@"c:\windows\file1.xml.bak", Utility.GetBackupFileName(@"c:\windows\file1.xml"));
        }

        [TestMethod]
        public void Utility_CreateBackupFileNormally()
        {

            string dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            string file1 = Path.Combine(dir, "file1.csproj");

            File.WriteAllText(file1, String.Empty);

            Utility.CreateBackupOf(file1);
            Assert.IsTrue(File.Exists(Utility.GetBackupFileName(file1)));
            Directory.Delete(dir, true);
        }

        [TestMethod]
        public void Utility_CreateBackupFileOverwriting()
        {

            string dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            string file1 = Path.Combine(dir, "file1.csproj");

            File.WriteAllText(file1, String.Empty);


            File.WriteAllText(Utility.GetBackupFileName(file1), String.Empty);
            
            Utility.CreateBackupOf(file1);
            Assert.IsTrue(File.Exists(Utility.GetBackupFileName(file1)));
            Directory.Delete(dir, true);
        }

        [TestMethod]
        
        public void Utility_CreateBackupFileOverwritingReadOnlyFiles()
        {

            string dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            string file1 = Path.Combine(dir, "file1.csproj");

            File.WriteAllText(file1, String.Empty);

            string existingFile = Utility.GetBackupFileName(file1);
            File.WriteAllText(existingFile, String.Empty);

            File.SetAttributes(existingFile, FileAttributes.ReadOnly);
            try
            {
                Utility.CreateBackupOf(file1);
            }
            catch (BackupFailedException ex)
            {
                Assert.AreEqual(String.Format("Unable to create backup of file '{0}'. Check if there is already a readonly file named '{1}'", file1, existingFile), ex.Message);
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                File.SetAttributes(existingFile, FileAttributes.Normal);
                Directory.Delete(dir, true);
            }
        }

        [TestMethod]
        public void CheckWildCardsFetchAllFiles()
        {

            string dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            string dir2 = Path.Combine(dir, "test");
            Directory.CreateDirectory(dir2);
            string file1 = Path.Combine(dir, "file1.csproj");
            string file2 = Path.Combine(dir, "file2.csproj");
            string file3 = Path.Combine(dir, "file3.xml");
            string file4 = Path.Combine(dir2, "file4.csproj");

            File.WriteAllText(file1, String.Empty);
            File.WriteAllText(file2, String.Empty);
            File.WriteAllText(file3, String.Empty);
            File.WriteAllText(file4, String.Empty);

            string fileName = Path.Combine(dir, "*.csproj");

            string[] files = Utility.GetApplicableFilesInDir(fileName);

            Assert.AreEqual(3, files.Length);
            Assert.AreEqual(file1, files[0]);
            Assert.AreEqual(file2, files[1]);
            Assert.AreEqual(file4, files[2]);

            Directory.Delete(dir, true);
        }

        [TestMethod]
        public void CheckNoWildCardsFetchSingleFile()
        {
            string dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            string dir2 = Path.Combine(dir, "test");
            Directory.CreateDirectory(dir2);
            string file1 = Path.Combine(dir, "file1.csproj");
            string file2 = Path.Combine(dir, "file2.csproj");
            string file3 = Path.Combine(dir, "file3.xml");
            string file4 = Path.Combine(dir2, "file1.csproj");

            File.WriteAllText(file1, String.Empty);
            File.WriteAllText(file2, String.Empty);
            File.WriteAllText(file3, String.Empty);
            File.WriteAllText(file4, String.Empty);

            string fileName = Path.Combine(dir, "file1.csproj");

            string[] files = Utility.GetApplicableFilesInDir(fileName);

            Assert.AreEqual(1, files.Length);
            Assert.AreEqual(file1, files[0]);

            Directory.Delete(dir, true);

        }
    }
}
