using System;
using System.Reflection;
using XmlSearchReplaceConsoleLib;
using XmlSearchReplaceLib;
using XmlSearchReplaceConsoleLib.Validator;

namespace XmlSearchReplaceConsole
{
    class Program
    {
        private static SearchReplaceFileReplacer _Main;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowUsage();
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
                return;
            }

            try
            {

                CommandlineParser parser = new CommandlineParser(args);

                DefaultParameterValidator validator = new DefaultParameterValidator();
                validator.CheckParameters(parser.GetParamsAndValues());                 
                _Main = new SearchReplaceFileReplacer(new ApplicationParameters(parser.GetParamsAndValues()));
                _Main.ProcessAll();
            }
            catch (InvalidArgumentOptionException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (RequiredParameterMissingException ex)
            {
                Console.WriteLine(ex.ToString());
                ShowUsage();
            }
            catch (BaseException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine(SearchReplaceFileReplacer.GetUsage(Assembly.GetEntryAssembly().GetName().Name + ".exe"));            
        }
    }    
}
