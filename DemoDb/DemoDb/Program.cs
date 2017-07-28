using System;

namespace DemoDb
{
    public class Program
    {
        public static void Main()
        {
            Initializer.Initialize();
            PopulateData.Start();

            DemoRunner.Start();

            Console.WriteLine("*** STARTING ***");
            Console.WriteLine("*** FINISHED ***");
            Console.ReadKey();
        }
    }
}