// See https://aka.ms/new-console-template for more information
using System;

namespace CodeGenConsole
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var foo = new FloopThePig5();
            Yolo();
            Console.WriteLine($"Y={foo.Yolo()}");
            HelloFrom("Generated Code");
        }

        public static void Yolo()
        {
            Console.WriteLine("Hello");
        }

        static partial void HelloFrom(string name);
    }
}
