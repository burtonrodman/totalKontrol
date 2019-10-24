namespace totalKontrol.Core.Commands
{
    public static class CommandFactory
    {
        public static ICommand Create(string name)
        {
            switch (name)
            {
                case "ChangeVolume":  return new ChangeVolumeCommand();
            }

            return null;
        }
    }
}
