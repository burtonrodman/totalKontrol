using System.Threading;

namespace totalKontrol.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var controller = new Core.MidiController(".\\nanoKONTROL2.controller", new ConsoleLogger()))
            {
                controller.Start();

                while (true)
                {
                    Thread.Sleep(30000);
                }

            }
        }
    }
}
