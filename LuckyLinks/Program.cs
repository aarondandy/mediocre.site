using System;
using System.Linq;

namespace LuckyLinks
{
    class Program
    {
        static void Main(string[] args) {

            var search = Console.ReadLine();
            Console.WriteLine("http://google.com/" + Escape("search") + '?' + Escape("btnI") + "&q=" + Escape(search));
            Console.WriteLine("Press the [Any] key.");
            Console.ReadKey();

        }

        static string Escape(string text) {
            return String.Concat(text.Select(c => {
                if (c == ' ')
                    return "+";
                return Uri.HexEscape(c);
            }));
        }

    }
}
