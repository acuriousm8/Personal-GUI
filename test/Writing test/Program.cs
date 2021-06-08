using System;
using System.Threading;
using WindowsInput;

namespace Writing_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            InputSimulator write = new InputSimulator();
            Thread.Sleep(2000);
            write.Keyboard.TextEntry("test"); //test

        }


    }
}
