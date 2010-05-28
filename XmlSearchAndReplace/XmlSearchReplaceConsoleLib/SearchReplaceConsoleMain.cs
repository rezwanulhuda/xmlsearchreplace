using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlSearchReplaceConsoleLib.Validator;

namespace XmlSearchReplaceConsoleLib
{
    public class SearchReplaceConsoleMain
    {

        public void Start(string[] args)
        {
            Start(String.Join(" ", args));
        }
        public void Start(string args)
        {
            CommandlineParser parser = new CommandlineParser(args);

            ValidateParameters(parser);
            SearchReplaceFileReplacer replacer = new SearchReplaceFileReplacer(new ApplicationParameters(parser.GetParamsAndValues()));
            replacer.ProcessAll();
        }

        private static void ValidateParameters(CommandlineParser parser)
        {
            DefaultParameterValidator validator = new DefaultParameterValidator();
            validator.CheckParameters(parser.GetParamsAndValues());
        }
    }
}
