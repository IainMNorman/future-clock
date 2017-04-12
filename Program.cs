using System;

namespace fc_core
{
    class Program
    {
        public static ulong InstallTime { get; set; }

        public static ulong CurrentTime { get; set; }

        public static double Power { get; set; }

        public static void Main(string[] args)
        {
            InstallTime = 1479289234;
            CurrentTime = InstallTime;
            Power = 8d;

            Console.Clear();

            RefreshFutureTime();
            do
            {
                var ki = Console.ReadKey(true);

                switch (ki.Key)
                {
                    case ConsoleKey.Q:
                        CurrentTime += 86400;
                        break;
                    case ConsoleKey.A:
                        CurrentTime -= 86400;
                        break;
                    case ConsoleKey.W:
                        CurrentTime += 3600;
                        break;
                    case ConsoleKey.S:
                        CurrentTime -= 3600;
                        break;
                    case ConsoleKey.E:
                        CurrentTime += 60;
                        break;
                    case ConsoleKey.D:
                        CurrentTime -= 60;
                        break;
                    case ConsoleKey.R:
                        CurrentTime++;
                        break;
                    case ConsoleKey.F:
                        CurrentTime--;
                        break;
                    case ConsoleKey.T:
                        Power += 0.1;
                        break;
                    case ConsoleKey.G:
                        Power -= 0.1;
                        break;
                    default:
                        break;
                }
                RefreshFutureTime();
            } while (true);
        }

        static void RefreshFutureTime()
        {
            var ticksSinceInstall = CurrentTime - InstallTime;
            var daysSinceInstall = ticksSinceInstall / 86400d;
            var daysInFuture = Math.Pow(Power, daysSinceInstall) - 2;
            var futureTime = CurrentTime + (ulong)(daysInFuture * 86400);
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("{0} => {1} / ({2})      ", new NormDateTime(CurrentTime), new NormDateTime(futureTime), Power);
        }
    }
}
