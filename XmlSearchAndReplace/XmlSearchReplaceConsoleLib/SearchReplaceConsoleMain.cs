using System;
using System.Xml;
using XmlSearchReplaceLib;
using System.IO;

namespace XmlSearchReplaceConsoleLib
{
    public class SearchReplaceFileReplacer
    {

        SearchReplaceParameter _Parameters = null;
        XmlSearchReplace _Replacer = null;
        XmlDocument _Document = null;

        public SearchReplaceFileReplacer(SearchReplaceParameter parameters)
        {            
            _Replacer = new XmlSearchReplace(
                parameters.GetLocationOptions()
                , parameters.GetOperationOptions()
                , parameters.GetSearchString()
                , parameters.GetReplaceString());
            this._Parameters = parameters;
            _Document = new XmlDocument();
        }

        public void ProcessAll()
        {
            foreach (string file in Utility.GetApplicableFilesInDir(_Parameters.GetFileName()))
            {
                ProcessFile(file);
            }
        }        

        private void ProcessFile(string file)
        {
            bool error = false;
            string fileName = Path.GetFileName(file);
            Console.Write("Processing: '{0}'...", fileName);
            try
            {                
                Replace(file);
            }            
            catch
            {                
                if (!_Parameters.ContinueOnError)
                    throw;
                error = true;
            }

            Console.WriteLine((error ? "Error. Check file is valid xml and not readonly." : "Successful."));
        }

        private void Replace(string file)
        {            
            _Document.Load(file);       
            XmlDocument replaced = _Replacer.Replace(_Document);
            BackupAndSaveDocument(file, replaced);
        }        

        private void BackupAndSaveDocument(string file, XmlDocument docReplaced)
        {
            Utility.CreateBackupOf(file);
            docReplaced.Save(file);            
        }

        public static string GetUsage(string hostExecutableName)
        {
            string usage = String.Format("{0} {1}", hostExecutableName, CommandLineParameterCollection.GetUsage()) + Environment.NewLine;
            usage += CommandLineParameterCollection.GetHelpText();
            return usage;

        }
       
    }
}
