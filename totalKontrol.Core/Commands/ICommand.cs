using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public interface ICommand
    {
        void Initialize(IDeviceLocator deviceLocator);
        void Execute(int value, ControlGroup controlGroup);
    }
}
