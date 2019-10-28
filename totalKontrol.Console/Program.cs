using System.Threading;
using totalKontrol.Core.Device;

namespace totalKontrol.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var _logger = new ConsoleLogger();
            using (var _locator = new MMDeviceLocator())
            {
                using (var controller = new Core.MidiController(
                    ".\\nanoKONTROL2.controller",
                    ".\\nanoKONTROL2.commands",
                    _logger, _locator))
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
}
