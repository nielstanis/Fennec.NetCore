using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace BasicConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var taint = args[0];
            if (System.IO.File.Exists(taint))
            {
                var x = System.IO.File.OpenRead(taint);
            }
            OtherMethod(taint);
        }
        
        static void OtherMethod(string input)
        {
            var otherstring = "test";
            if (System.IO.File.Exists(otherstring))
            {
                var u = System.IO.File.OpenRead(otherstring);
                var bin = new BinaryFormatter();
                var instance = bin.Deserialize(u);
            }
        }
    }
}
