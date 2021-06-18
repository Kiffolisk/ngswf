using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace NGSWF
{
    class Program
    {
        static int curSelected = 0;
        
        static string Option(int id, string optionText)
        {
            return (curSelected == 0 ? "[-]" : $"[{id + 1}]") + $" {optionText}";
        }

        static void ActualThing()
        {
            Console.Clear();
            Console.WriteLine("-- NGSWF --");
            Console.WriteLine("Press ENTER to use.");
            Console.WriteLine("\n");
            PrintOptions();
            InputManager();
        }

        // https://www.newgrounds.com/portal/view/497632?emulate=flash

        static void PrintOptions()
        {
            Console.WriteLine(Option(0, "Download SWF from Newgrounds Link"));
        }

        static void GetSWFFromURL(string url)
        {
            var request = WebRequest.Create(url);
            request.Method = "POST";

            var response = request.GetResponse();
            // Console.WriteLine(((HttpWebResponse)response).StatusCode);

            var respStream = response.GetResponseStream();

            var reader = new StreamReader(respStream);
            string data = reader.ReadToEnd();

            bool has = data.Contains("swf: \"");
            Console.WriteLine((data.Contains("swf: \"") ? "SWF file reference exists." : "SWF file reference doesn't exist! Trying to add ?emulate=flash to url..."));
            if (has)
            {
                Console.WriteLine("SWF File URL: " + data.GetFrom("swf: \"".Length, "swf: \"").GetUntilOrEmpty("?"));
                string swfUrl = data.GetFrom("swf: \"".Length, "swf: \"").GetUntilOrEmpty("?");
                // download: new WebClient().DownloadFile
                string swfDownloadFolder = $@"C:\Users\{Environment.UserName}\Documents\Downloaded NG SWFs\";
                if (!Directory.Exists(swfDownloadFolder))
                {
                    Directory.CreateDirectory(swfDownloadFolder);
                }
                // Console.WriteLine(swfUrl.GetFrom(8, "https://").GetFrom(1, "/").GetFrom(1, "/"));
                new WebClient().DownloadFile(swfUrl, swfDownloadFolder + swfUrl.GetFrom(8, "https://").GetFrom(1, "/").GetFrom(1, "/"));
                Process.Start(swfDownloadFolder);
            }
            else if (!has && !url.EndsWith("?emulate=flash"))
            {
                GetSWFFromURL(url + "?emulate=flash");
            }
            else
            {
                Console.WriteLine("Sorry, the program could not find the SWF URL.");
            }
        }

        static void InputManager()
        {
            // https://uploads.ungrounded.net/183000/183575_ufa.swf
            ConsoleKeyInfo input = Console.ReadKey();
            if (input.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                Console.WriteLine("Paste the Newgrounds game link:");
                string link = Console.ReadLine();

                GetSWFFromURL(link);
            }
            ActualThing();
        }

        static void Main(string[] args)
        {
            ActualThing();
        }
    }
    static class Helper
    {
        public static string GetUntilOrEmpty(this string text, string stopAt = "-")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }

        public static string GetFrom(this string text, int offset, string stopAt = "-")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);
                    
                return text.Substring(charLocation + offset);
            }

            return String.Empty;
        }
    }
}
