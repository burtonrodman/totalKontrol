using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using totalKontrol.Core;
using totalKontrol.Core.Device;

namespace totalKontrol.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        MidiController _midiController;
        IDeviceLocator _deviceLocator;
        Dictionary<string, ControlGroupUserControl> _controlGroups;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _deviceLocator = new MMDeviceLocator();
            _midiController = new MidiController(
                ".\\nanoKONTROL2.controller", ".\\nanoKONTROL2.commands",
                new NullLogger(), _deviceLocator);

            _midiController.ControlChanged += _midiController_ControlChanged;

            _controlGroups = new Dictionary<string, ControlGroupUserControl>()
            {
                { "Group 1", this.Group1 },
                { "Group 2", this.Group2 },
                { "Group 3", this.Group3 },
                { "Group 4", this.Group4 },
                { "Group 5", this.Group5 },
                { "Group 6", this.Group6 },
                { "Group 7", this.Group7 },
                { "Group 8", this.Group8 }
            };

            _midiController.Start();
        }

        private void _midiController_ControlChanged(object sender, ControlChangedEventArgs e)
        {
            if (_controlGroups.TryGetValue(e.ControlGroup?.Name, out var control))
            {
                control.HandelControlChangedEvent(e);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _midiController.Dispose();
            _deviceLocator.Dispose();
        }
    }

    public class NullLogger : ILogger
    {
        public void WriteLine(string message)
        {
            // throw new NotImplementedException();
        }
    }
}
