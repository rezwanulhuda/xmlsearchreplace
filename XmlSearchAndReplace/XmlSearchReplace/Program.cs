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
                _Main = new SearchReplaceConsoleMain(args);
                _Main.ProcessAll();
            }
            catch (InvalidArgumentOptionException ex)
            {
                Console.WriteLine(ex.Message);
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
