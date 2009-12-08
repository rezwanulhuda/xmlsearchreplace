using System;
using System.Reflection;
using XmlSearchReplaceConsoleLib;
using XmlSearchReplaceLib;

namespace XmlSearchReplaceConsole
{
    class Program
    {
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
                SearchReplaceConsoleMain main = new SearchReplaceConsoleMain(args);
                main.ProcessAll();
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

            Console.WriteLine("Usage:" + Environment.NewLine);

            string usage = Assembly.GetEntryAssembly().GetName().Name;

            usage += "/O=ev,en,av /F={filename} /S={search string} /R={replace with string} /W /I /C" + Environment.NewLine;
            usage += "O - Options for search and replace. Available options are: av - attribute value, ev - element value, en = element name, an = attribute name" + Environment.NewLine;
            usage += "S - string to search" + Environment.NewLine;
            usage += "R - string to replace with" + Environment.NewLine;
            usage += "F - file name to perform the operation. Can use wildcards. Always searches all files in sub directory" + Environment.NewLine;
            usage += "W - matches whole word only" + Environment.NewLine;
            usage += "I - case insensitive search" + Environment.NewLine;
            usage += "C - continue on error when processing multiple files" + Environment.NewLine;

            // TODO features            
            // 3. Commandline argument validation            
            // 5. Regex support...

            Console.WriteLine(usage);
        }
    }
}
