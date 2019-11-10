namespace totalKontrol.Core.Commands
{
    public class PlayPauseTransportCommand : ButtonCommandBase
    {

        protected override (bool shouldExecute, bool isIlluminated) OnPress()
        {
            // only works if you also send the USB Keyboard scan code
            KeyboardHelpers.SendKeyPress(KeyCode.MEDIA_PLAY_PAUSE, 0xe8);
            return (false, true);
        }

    }
}
