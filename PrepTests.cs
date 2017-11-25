using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace PrepTests
{
    public static class Tests
    {
        public static void RunTests()
        {
            TestInfListener();
            // TestKeyAvailable();
        }

        private static void TestKeyAvailable()
        {
            List<char> inThread = new List<char>(),
                       inFunction = new List<char>();
            
            new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        while (!Console.KeyAvailable);
                        char key = Console.ReadKey().KeyChar;
                        inThread.Add(key);
                        if (key == 'q')
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tq in tread");
                            break;
                        }
                    }

                    Console.WriteLine("\tthread:");
                    inThread.ForEach(Console.Write);
                })).Start();
            
            while (true)
            {
                while (!Console.KeyAvailable);
                char key = Console.ReadKey().KeyChar;
                inFunction.Add(key);
                if (key == 'q')
                {
                    Console.WriteLine();
                    Console.WriteLine("\tq in function");
                    break;
                }
            }

            Console.WriteLine("\tfunction:");
            inFunction.ForEach(Console.Write);
            Console.WriteLine();
        }

        private static void TestInfListener()
        {
            Console.WriteLine("Testing infinite listener. \'q\' aborts execution.");

            bool exit = false;
            ConsoleKeyInfo k;
            new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        if (!exit && k.KeyChar == 'q')
                        {
                            exit = true;
                            break;
                        }
                    }
                })).Start();

            while (true)
            {
                if (exit)
                    break;
                k = Console.ReadKey();
                Console.WriteLine(" - {0} / {1} ({2})",
                    k.Modifiers,
                    k.Key,
                    k.KeyChar);
            }

            Console.WriteLine("Aborted.");
        }
    }
}
