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
            }

            return null;
        }
    }
}
