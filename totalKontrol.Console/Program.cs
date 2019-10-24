using System.Threading;

namespace totalKontrol.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var _logger = new ConsoleLogger();
            using (var controller = new Core.MidiController(
                ".\\nanoKONTROL2.controller",
                ".\\nanoKONTROL2.commands",
                _logger))
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
