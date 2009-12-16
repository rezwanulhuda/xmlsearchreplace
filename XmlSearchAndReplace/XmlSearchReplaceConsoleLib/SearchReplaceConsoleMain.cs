using System;
using System.Xml;
using XmlSearchReplaceLib;
using System.IO;

namespace XmlSearchReplaceConsoleLib
{
    public class SearchReplaceConsoleMain
    {

        ArgumentParser _Parser = null;
        XmlSearchReplace _Replacer = null;
        XmlDocument _Document = null;

        public SearchReplaceConsoleMain(string args)
        {
            _Parser = new ArgumentParser(args);
            _Replacer = new XmlSearchReplace(
                _Parser.GetLocationOptions()
                , _Parser.GetOperationOptions()
                , _Parser.GetSearchString()
                , _Parser.GetReplaceString());
            _Document = new XmlDocument();
        }

        public SearchReplaceConsoleMain(string[] args)
            : this(String.Join(" ", args))
        {            
        }

        public void ProcessAll()
        {
            foreach (string file in Utility.GetApplicableFilesInDir(_Parser.GetFileName()))
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
                if (!_Parser.ContinueOnError)
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
