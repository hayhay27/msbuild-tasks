using System;
using System.Text.RegularExpressions;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var d = "./build/_build.csproj";
            var r = Regex.IsMatch(d, "_build");
            Console.WriteLine(r ? "Match" : "Not match");
            Console.WriteLine("Hello World!");
        }
    }
}