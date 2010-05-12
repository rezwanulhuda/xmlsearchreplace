using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XmlSearchReplaceConsoleLib
{
    public class FileParamReader
    {

        string _ParameterFileName;
        public FileParamReader(string parameterFileName)
        {
            if (String.IsNullOrEmpty(parameterFileName))
            {
                throw new InvalidArgumentOptionException("Parameter file name cannot be empty");
            }

            if (!File.Exists(parameterFileName))
            {
                throw new InvalidArgumentOptionException("Parameter file does not exist");                
            }

            _ParameterFileName = parameterFileName;

            LoadFromFile();
        }

        List<string> _SearchStrings = new List<string>();
        List<string> _ReplaceStrings = new List<string>();

        private void LoadFromFile()
        {
            string[] lines = File.ReadAllLines(_ParameterFileName);

            foreach (string line in lines)
            {
                CommandlineParser parser = new CommandlineParser(line);
                ApplicationParameterWithValueCollection values = parser.GetParamsAndValues();

                _SearchStrings.Add(values.GetSearchString()[0]);
                _ReplaceStrings.Add(values.GetReplaceString()[0]);
            }
        }

        public List<string> GetAllSearchStrings()
        {            
            return _SearchStrings;
        }

        public List<string> GetAllReplaceStrings()
        {         
            return _ReplaceStrings;
        }
    }
}
