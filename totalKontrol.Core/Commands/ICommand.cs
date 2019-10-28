using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public interface ICommand
    {
        void Execute(int value, ControlGroup controlGroup);
    }
}
