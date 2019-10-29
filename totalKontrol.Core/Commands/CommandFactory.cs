namespace totalKontrol.Core.Commands
{
    public static class CommandFactory
    {
        public static ICommand Create(string name, IDeviceLocator deviceLocator)
        {
            switch (name)
            {
                case "ChangeOutVolume":  return new ChangeOutVolumeCommand(deviceLocator);
                case "MuteOutVolume": return new MuteOutVolumeCommand(deviceLocator);
                case "PlayPauseTransport": return new PlayPauseTransportCommand();
                case "StopTransport": return new StopTransportCommand();
                case "TrackNext": return new TrackNextCommand();
            }

            return null;
        }
    }
}
