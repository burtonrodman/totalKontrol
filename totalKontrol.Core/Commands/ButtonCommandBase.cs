using System;
using totalKontrol.Core.Definition;
using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public abstract class ButtonCommandBase : ICommand
    {
        private MidiController _midiController;
        protected Control Control { get; set; }
        protected ILogger Logger { get; set; }
        protected ControlGroup ControlGroup { get; set; }
        protected IDeviceLocator DeviceLocator { get; set; }

        public void Initialize(Control control, ControlGroup controlGroup, MidiController midiController, ILogger logger, IDeviceLocator deviceLocator)
        {
            _midiController = midiController;
            this.Control = control;
            this.Logger = logger;
            this.ControlGroup = controlGroup;
            this.DeviceLocator = deviceLocator;
        }

        public bool ProcessEvent(int value)
        {
            switch (value)
            {
                case 127: return OnPressInternal(OnPress, "pressed");
                case 0: return OnPressInternal(OnRelease, "released");
            }
            return false;
        }

        protected virtual (bool shouldExecute, bool isIlluminated) OnPress() => (false, false);
        protected virtual (bool shouldExecute, bool isIlluminated) OnRelease() => (false, false);

        private bool OnPressInternal(Func<(bool shouldExecute, bool isIlluminated)> func, string action)
        {
            var (shouldExecute, isIlluminated) = func.Invoke();
            Logger.WriteLine($"button {Control.Name} {action}");
            _midiController.OnControlChanged(new ControlChangedEventArgs() { ControlGroup = ControlGroup, ControlName = Control.Name, IsPressed = isIlluminated });
            SetLight(isIlluminated);
            return shouldExecute;
        }

        protected virtual void OnExecuteCommand() { }

        public void ExecuteCommand()
        {
            OnExecuteCommand();
        }

        protected void SetLight(bool isIlluminated)
        {
            _midiController.SendControlChange(Control.Controller, isIlluminated ? 127 : 0);
        }

    }
}
