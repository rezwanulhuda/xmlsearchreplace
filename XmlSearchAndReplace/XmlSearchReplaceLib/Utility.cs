using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace XmlSearchReplaceLib
{
    public static class Utility
    {
        public static string GetBackupFileName(string fileName)
        {
            return Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileName(fileName) + ".bak"); 
        }

        public static string[] GetApplicableFilesInDir(string fileName, bool recurseSubDir)
        {
            return Directory.GetFiles(Path.GetDirectoryName(fileName), Path.GetFileName(fileName), recurseSubDir ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public static void CreateBackupOf(string fileName)
        {
            string dirName = Path.GetDirectoryName(fileName);
            string backupFileName = Utility.GetBackupFileName(fileName);

            try
            {
                if (File.Exists(backupFileName))
                    File.Delete(backupFileName);


                File.Copy(fileName, backupFileName);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new BackupFailedException(
                    String.Format("Unable to create backup of file '{0}'. Check if there is already a readonly file named '{1}'", fileName, backupFileName)
                    , ex
                    );
            }
        }

        public static bool IsValidXmlName(string str)
        {
            Regex validXmlName = new Regex(@"^(?!(xml|[_\d\W]))[^ \s\W]+$");
            return validXmlName.IsMatch(str);
        }
    }
}
