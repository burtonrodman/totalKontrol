namespace totalKontrol.Core.Commands
{
    public interface ICommand
    {
        void Execute(int value, string[] parameters);
    }
}
