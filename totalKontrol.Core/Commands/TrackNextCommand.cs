namespace totalKontrol.Core.Commands
{
    public class TrackNextCommand : ButtonCommandBase
    {

        protected override (bool shouldExecute, bool isIlluminated) OnPress()
        {
            // only works if you also send the USB Keyboard scan code
            KeyboardHelpers.SendKeyPress(KeyCode.MEDIA_NEXT_TRACK, 0xeb);
            return (false, true);   
        }

    }
}
