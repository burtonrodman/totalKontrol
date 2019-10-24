namespace totalKontrol.Core.Commands
{
    public static class CommandFactory
    {
        public static ICommand Create(string name)
        {
            switch (name)
            {
                case "ChangeOutVolume":  return new ChangeOutVolumeCommand();
                case "MuteOutVolume": return new MuteOutVolumeCommand();
            }

            return null;
        }
    }
}
