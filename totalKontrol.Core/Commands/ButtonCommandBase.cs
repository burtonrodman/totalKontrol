using totalKontrol.Core.Definition;
using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public abstract class ButtonCommandBase : ICommand
    {
        private Control _control;
        private MidiController _midiController;
        private ILogger _logger;
        protected ControlGroup ControlGroup { get; set; }
        protected IDeviceLocator DeviceLocator { get; set; }

        public void Initialize(Control control, ControlGroup controlGroup, MidiController midiController, ILogger logger, IDeviceLocator deviceLocator)
        {
            _control = control;
            _midiController = midiController;
            _logger = logger;
            this.ControlGroup = controlGroup;
            this.DeviceLocator = deviceLocator;
        }

        public bool ProcessEvent(int value)
        {
            switch (value)
            {
                case 0: return OnRelease(); 
                case 127: return OnPress(); 
            }
            return false;
        }
        
        protected virtual void OnExecuteCommand()
        {
        }

        protected virtual bool OnPress()
        {
            _logger.WriteLine($"button {_control.Name} pressed");
            _midiController.OnControlChanged(new ControlChangedEventArgs() { ControlGroup = ControlGroup, ControlName = _control.Name, IsPressed = true });
            SetLightOn();
            return false;
        }

        protected virtual bool OnRelease()
        {
            _logger.WriteLine($"button {_control.Name} released");
            _midiController.OnControlChanged(new ControlChangedEventArgs() { ControlGroup = ControlGroup, ControlName = _control.Name, IsPressed = false });
            SetLightOff();
            return false;
        }

        public void ExecuteCommand()
        {
            OnExecuteCommand();
        }

        protected void SetLightOn()
        {
            _midiController.SendControlChange(_control.Controller, 127);
        }

        protected void SetLightOff()
        {
            _midiController.SendControlChange(_control.Controller, 0);
        }

    }
}
