using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public class StopTransportCommand : ButtonCommandBase
    {
        protected override void OnPress(int value, ControlGroup controlGroup)
        {
            base.OnPress(value, controlGroup);

            // only works if you also send the USB Keyboard scan code
            KeyboardHelpers.SendKeyPress(KeyCode.MEDIA_STOP, 0xf3);
        }
    }
}
