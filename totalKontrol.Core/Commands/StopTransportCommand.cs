using totalKontrol.Core.Definition;
using totalKontrol.Core.Profile;

namespace totalKontrol.Core.Commands
{
    public class StopTransportCommand : ButtonCommandBase
    {

        protected override bool OnPress()
        {
            base.OnPress();

            // only works if you also send the USB Keyboard scan code
            KeyboardHelpers.SendKeyPress(KeyCode.MEDIA_STOP, 0xf3);
            return false;
        }

    }
}
