using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZebraScannerWrapper.Data;
using ZebraScannerWrapper.Enums;

namespace ZebraScannerWrapper.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScannerManager _manager;
        private Scanner _selectedScanner;


        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Setup enums
            BeepComboBox.ItemsSource = Enum.GetValues(typeof(BeepType)).Cast<BeepType>();
            LEDComboBox.ItemsSource = Enum.GetValues(typeof(LEDColor)).Cast<LEDColor>();

            _manager = new ScannerManager(ScannerManager.DefaultSupportedScannerTypes, true, false);

            UpdateListBox();
            _manager.RegisterPNPCallback(RecievePnp);
            _manager.RegisterBarcodeCallback(RecieveScan);
            _manager.RegisterWeightCallback(RecieveWeightLive);
        }
        private void RecieveWeightLive(WeightData weight)
        {

        }

        private void RecieveScan(ScanData scanData) 
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                LogListBox.Items.Add($"LOG: BARCODE: Scanner {scanData.Scanner.ModelNumber} scanned type: {scanData.BarcodeType.ToString()} data: {scanData.Barcode}");
            })); 
        }

        private void RecievePnp(PNPData pnpData)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                LogListBox.Items.Add($"LOG: PNP: Scanner {pnpData.Scanner.ModelNumber} has event {pnpData.PNPEvent.ToString()}");
                UpdateListBox();
            }));
            
        }

        private void UpdateListBox()
        {
            ScannerListBox.Items.Clear();
            foreach (var scan in _manager.GetScanners()) { ScannerListBox.Items.Add(scan); }
        }

        private void ScannerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Scanner scan = ScannerListBox.SelectedItem as Scanner ;
            if(scan != null) 
            {
                SelectedScannerField.Text = "SERIAL: " + scan.SerialNumber + " NAME: " + scan.ModelNumber;
                _selectedScanner = scan;
            }
        }

        private void PullTrigger_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedScanner != null)
            {
                _selectedScanner.PullTrigger();
            }
        }

        private void ReleaseTrigger_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedScanner != null)
            {
                _selectedScanner.ReleaseTrigger();
            }
        }

        private void AimOn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                _selectedScanner.AimOn();
            }
        }

        private void AimOff_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                _selectedScanner.AimOff();
            }
        }

        private void Reboot_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedScanner != null)
            {
                _selectedScanner.Reboot();
            }
        }

        private void ScaleZero_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                _selectedScanner.ScaleZero();
            }
        }

        private void ScaleReset_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                _selectedScanner.ScaleReset();
            }
        }

        private void GetWeight_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                WeightText.Text = "new weight";
            }
        }

        private void Beep_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                _selectedScanner.PlayBeep((BeepType)LEDComboBox.SelectedItem);
            }
        }

        private void LED_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                _selectedScanner.SetLEDColor((LEDColor)LEDComboBox.SelectedItem);
            }
        }
    }
}