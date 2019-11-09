using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public abstract class ButtonCommandBase : ICommand
    {
        public void Initialize(IDeviceLocator deviceLocator)
        {
            OnInitialize(deviceLocator);
        }

        public void Execute(int value, ControlGroup controlGroup)
        {
            switch (value)
            {
                case 0: OnRelease(value, controlGroup); break;
                case 127: OnPress(value, controlGroup); break;
            }
        }

        protected virtual void OnInitialize(IDeviceLocator deviceLocator)
        {
        }

        protected virtual void OnPress(int value, ControlGroup controlGroup)
        {
        }

        protected virtual void OnRelease(int value, ControlGroup controlGroup)
        {
        }
    }
}
