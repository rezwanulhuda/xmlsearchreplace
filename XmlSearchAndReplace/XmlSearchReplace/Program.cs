using System;
using System.Reflection;
using XmlSearchReplaceConsoleLib;
using XmlSearchReplaceLib;
using XmlSearchReplaceConsoleLib.Validator;

namespace XmlSearchReplaceConsole
{
    class Program
    {
        [PreEmptive.Attributes.Setup(CustomEndpoint = "so-s.info/PreEmptive.Web.Services.Messaging/MessagingServiceV2.asmx")]
        [PreEmptive.Attributes.Teardown()]
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
                SearchReplaceConsoleMain main = new SearchReplaceConsoleMain();
                main.Start(args);
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
            Console.WriteLine(SearchReplaceFileReplacer.GetUsage(Assembly.GetEntryAssembly().GetName().Name + ".exe"));            
        }
    }    
}
