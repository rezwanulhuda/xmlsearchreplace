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
            string execName = Assembly.GetEntryAssembly().GetName().Name + ".exe ";
            string usage = execName;

            usage += "/O=ev,en,av,an /F={filename} /S={search string} /R={replace with string} /W /I /C" + Environment.NewLine;
            usage += "O - Options for search and replace. Available options are:" + Environment.NewLine;
            usage += "  av - Attribute value" + Environment.NewLine;
            usage += "  ev - Element value" + Environment.NewLine;
            usage += "  en - Element name" + Environment.NewLine;
            usage += "  an - Attribute name" + Environment.NewLine;
            usage += "S - String to search" + Environment.NewLine;
            usage += "R - String to replace with" + Environment.NewLine;
            usage += "F - File name to perform the operation. Can use wildcards. Always searches all files in sub directory if wildcards are used." + Environment.NewLine;
            usage += "W - Matches whole word only" + Environment.NewLine;
            usage += "I - Case insensitive search" + Environment.NewLine;
            usage += "C - Continue on error when processing multiple files" + Environment.NewLine;
            usage += "Example:" + Environment.NewLine;
            usage += execName + @" /O=ev,av /S=""Book"" /R=""LibraryBook"" /F=""C:\xmls\*.csproj"" /W /I /C";
            usage += "see project documentation for further details. http://xmlsearchreplace.codeplex.com/documentation";

            // TODO features            
            // 3. Commandline argument validation            
            // 5. Regex support...

            Console.WriteLine(usage);
        }
    }
}
