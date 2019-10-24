using totalKontrol.Core;

namespace totalKontrol.Console
{
    public class ConsoleLogger : ILogger
    {
        public void WriteLine(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}
