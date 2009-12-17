using System;
using System.Reflection;
using XmlSearchReplaceConsoleLib;
using XmlSearchReplaceLib;

namespace XmlSearchReplaceConsole
{
    class Program
    {
        private static SearchReplaceConsoleMain _Main;
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
                ArgumentParser parser = new ArgumentParser(args);
                SearchReplaceParameter param = new SearchReplaceParameter(parser.GetParamsAndValues());
                _Main = new SearchReplaceConsoleMain(param);
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
            Console.WriteLine(SearchReplaceConsoleMain.GetUsage(Assembly.GetEntryAssembly().GetName().Name + ".exe"));            
        }
    }
}
