using totalKontrol.Core.Definition;
using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public interface ICommand
    {
        void Initialize(Control control, ControlGroup controlGroup, MidiController midiController, ILogger logger, IDeviceLocator deviceLocator);
        bool ProcessEvent(int value);
        void ExecuteCommand();
    }
}
