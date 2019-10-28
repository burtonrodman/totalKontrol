using System.Windows.Controls;
using System.Windows.Media;
using totalKontrol.Core;

namespace totalKontrol.Wpf
{
    /// <summary>
    /// Interaction logic for ControlGroupUserControl.xaml
    /// </summary>
    public partial class ControlGroupUserControl : UserControl
    {
        ControlGroupViewModel _viewModel;

        public ControlGroupUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            _viewModel = new ControlGroupViewModel();
            this.DataContext = _viewModel;

        }

        public void HandelControlChangedEvent(ControlChangedEventArgs args)
        {
            if (args.ControlName.StartsWith("MUTE"))
            {
                _viewModel.MuteButtonBrush = args.IsIlluminated ? Brushes.Orange : Brushes.Black;
            }
            if (args.ControlName.StartsWith("SOLO"))
            {
                _viewModel.SoloButtonBrush = args.IsIlluminated ? Brushes.Orange : Brushes.Black;
            }
            if (args.ControlName.StartsWith("ARM"))
            {
                _viewModel.ArmButtonBrush = args.IsIlluminated ? Brushes.Orange : Brushes.Black;
            }
            if (args.ControlName.StartsWith("KNOB"))
            {
                _viewModel.KnobValue = args.Value;
            }
            if (args.ControlName.StartsWith("FADER"))
            {
                _viewModel.FaderValue = args.Value;
            }
        }
    }

    public class ControlGroupViewModel : ViewModelBase
    {

        private Brush _muteButtonBrush = Brushes.Black;
        public Brush MuteButtonBrush
        {
            get => _muteButtonBrush;
            set => SetProperty(ref _muteButtonBrush, value);
        }

        private Brush _soloButtonBrush = Brushes.Black;
        public Brush SoloButtonBrush
        {
            get => _soloButtonBrush;
            set => SetProperty(ref _soloButtonBrush, value);
        }

        private Brush _armButtonBrush = Brushes.Black;
        public Brush ArmButtonBrush
        {
            get => _armButtonBrush;
            set => SetProperty(ref _armButtonBrush, value);
        }

        private int _knobValue = 0;
        public int KnobValue
        {
            get => _knobValue;
            set => SetProperty(ref _knobValue, value);
        }

        private int _faderValue = 0;
        public int FaderValue
        {
            get => _faderValue;
            set => SetProperty(ref _faderValue, value);
        }
    }
}
