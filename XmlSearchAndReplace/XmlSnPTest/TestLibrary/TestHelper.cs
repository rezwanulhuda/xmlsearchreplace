using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using XmlSearchReplaceConsoleLib;

namespace XmlSnRTest
{
    public static class TestHelper
    {
        private static string _LastParamFile;
        public static string CreateParameterFile(params string[] values)
        {
            _LastParamFile = Path.GetTempFileName();
            File.WriteAllLines(_LastParamFile, values);
            return _LastParamFile;
        }

        public static void DeleteLastParameterFile()
        {
            if (File.Exists(_LastParamFile))
                File.Delete(_LastParamFile);
        }

        public static ISearchReplaceParameter GetParameters(string[] commandLine)
        {
            return GetApplicationParameters(String.Join(" ", commandLine));
        }
        public static ISearchReplaceParameter GetApplicationParameters(string commandLine)
        {            

            return new ApplicationParameters(GetCommandLineParameters(commandLine));
            //return parser.GetParamsAndValues();
        }

        public static CommandLineParameterWithValueCollection GetCommandLineParameters(string commandLine)
        {
            CommandlineParser parser = new CommandlineParser(commandLine);

            return parser.GetParamsAndValues();            
        }
    }
}
