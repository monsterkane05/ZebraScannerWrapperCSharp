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
        private readonly ScannerManager _manager;
        private Scanner? _selectedScanner = null;


        public MainWindow()
        {
            _manager = new ScannerManager(ScannerManager.DefaultSupportedScannerTypes, true, true);

            InitializeComponent();
            Loaded += MainWindow_Loaded;
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Setup enums
            BeepComboBox.ItemsSource = Enum.GetValues(typeof(BeepType)).Cast<BeepType>();
            LEDComboBox.ItemsSource = Enum.GetValues(typeof(LEDColor)).Cast<LEDColor>();

            

            UpdateListBox();
            _manager.RegisterPNPCallback(RecievePnp);
            _manager.RegisterBarcodeCallback(RecieveScan);
            _manager.RegisterWeightCallback(RecieveWeightLive);
            _manager.RegisterLiveWeightStatusCallback(RecieveLiveWeightStatus);
        }

        private void RecieveLiveWeightStatus(bool enabled)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                LogListBox.Items.Add($"LiveWeightStatus: enabled = {enabled}");
            }));
        }

        private void RecieveWeightLive(WeightData weight)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                LiveWeightTextBox.Text = weight.Weight + "  " +  weight.WeightUnit.ToString() + "  " + weight.WeightStatus.ToString();

                if (_manager.IsLiveWeightRunning())
                {
                    LiveWeightButton.Content = "STOP";
                }
                else
                {
                    LiveWeightButton.Content = "START";
                }
            }));
        }

        private void RecieveScan(ScanData scanData) 
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                LogListBox.Items.Add($"LOG: BARCODE: Scanner {scanData.Scanner.ModelNumber} scanned type: {scanData.BarcodeType} data: {scanData.Barcode}");
            })); 
        }

        private void RecievePnp(PNPData pnpData)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                LogListBox.Items.Add($"LOG: PNP: Scanner {pnpData.Scanner.ModelNumber} has event {pnpData.PNPEvent}");
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
            if (ScannerListBox.SelectedItem is Scanner scan)
            {
                SelectedScannerField.Text = "SERIAL: " + scan.SerialNumber + " NAME: " + scan.ModelNumber;
                _selectedScanner = scan;
            }
        }

        private void PullTrigger_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.PullTrigger();
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }

        private void ReleaseTrigger_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.ReleaseTrigger();
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }

        private void AimOn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.AimOn();
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }

        private void AimOff_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.AimOff();
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }

        private void Reboot_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.Reboot();
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }

        private void ScaleZero_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.ScaleZero();
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }

        private void ScaleReset_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.ScaleReset();
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }

        private void GetWeight_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.GetScaleWeight();
                if (response.Response == ScannerStatus.SUCCESS)
                {
                    WeightData? data = (WeightData?)response.ResponseData;
                    if (data != null)
                    {
                        WeightText.Text = data.Weight + data.WeightUnit.ToString();
                    }
                }
                else
                {
                    MessageBox.Show(response.Response.ToString());
                }
            }
        }

        private void Beep_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.PlayBeep((BeepType)LEDComboBox.SelectedItem);
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }

        private void LED_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.SetLEDColor((LEDColor)LEDComboBox.SelectedItem);
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }

        private void StartLive_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                
                if (_manager.IsLiveWeightRunning())
                {
                    _manager.StopLiveWeight();
                    LiveWeightButton.Content = "START";
                    LiveWeightTextBox.Text = "Live weight is not enabled";
                }
                else
                {
                    _manager.StartLiveWeight(_selectedScanner);
                }
                
            }
        }

        private void Enable_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.SetEnabled(true);
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }

        private void Disable_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScanner != null)
            {
                ScannerResponse response = _selectedScanner.SetEnabled(false);
                if (response.Response != ScannerStatus.SUCCESS) { MessageBox.Show(response.Response.ToString()); }
            }
        }
    }
}