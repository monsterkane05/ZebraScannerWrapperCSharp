using System.Net;
using ZebraScannerWrapper.Data;
using ZebraScannerWrapper.Enums;

namespace ZebraScannerWrapper
{
    public class ScannerManager
    { 
        //Static Configuration Array
        public static List<ScannerType> DefaultSupportedScannerTypes = new List<ScannerType>() { ScannerType.ALL };


        //Callback Definitions
        private List<Action<ScanData>> _barcodeScanCallbacks = new List<Action<ScanData>>();
        private List<Action<WeightData>> _weightDataCallbacks = new List<Action<WeightData>>();
        private List<Action<PNPData>> _pnpDataCallbacks = new List<Action<PNPData>>();

        private List<Scanner> _scanners = new List<Scanner>();

        //Management Properties
        private bool _customLEDManagementEnabled;
        private bool _liveWeightSubsystemEnabled;

        private CoreScannerLink _scannerLink;

        //Constructor
        public ScannerManager(List<ScannerType> SupportedScannerTypes, bool EnableCustomLEDManagement, bool EnableLiveWeightSubsystem)
        {
            _customLEDManagementEnabled = EnableCustomLEDManagement;
            _liveWeightSubsystemEnabled = EnableLiveWeightSubsystem;

            _scannerLink = new CoreScannerLink(this);

            int res = _scannerLink.ConfigureCoreScanner(SupportedScannerTypes.ToArray());
            if (res == 0) 
            {
                Console.WriteLine("Configured Core Scanner Successfully");

                //Setup Callbacks to barcode and pnp events
                _scannerLink.RegisterEvents(InternalPNPEvent, InternalBarcodeEvent);


                UpdateInternalScannerList();
            }
            else 
            {
                throw new Exception();
            }

        }

        private void UpdateInternalScannerList() 
        {
            List<Scanner> newList = _scannerLink.GetScannerList();

            //Compare the old list to new list and update new scanners. must keep there enabled status if they were already set.
            foreach (Scanner newScanner in newList) 
            {
                Scanner? scan = _scanners.Find(x => x.SerialNumber == newScanner.SerialNumber);
                if (scan != null)
                {
                    newScanner.SetEnabledInternal(scan.GetEnabled());
                }
            }

            //Replace with new array
            _scanners = newList;

            //Attempt to update the LED colors if this option is enabled on any scanner
            InternalUpdateLEDEvent();
        }

        private void InternalUpdateLEDEvent() 
        {
            if (_customLEDManagementEnabled) 
            {
                foreach(Scanner scan in _scanners) 
                {
                    LEDColor col = LEDColor.RED_ON;
                    if (scan.GetEnabled()) { col = LEDColor.GREEN_ON; }
                    _scannerLink.SetEnabledScanner(scan, scan.GetEnabled());
                    _scannerLink.SetLEDColor(scan, col);
                }
            }
        }

        private void InternalBarcodeEvent(short EventType, string RawScanData) 
        {
        
        }

        private void InternalPNPEvent(short EventType, string PNPData) 
        {
            UpdateInternalScannerList();

            foreach(Action<PNPData> action in _pnpDataCallbacks) 
            {
                //Process all callbacks wanting plug and play data.

            }
        }

        //Scanner Location Commands
        public Scanner? GetScannerBySerialNumber(string serialNumber) { return _scanners.Where(x => x.SerialNumber?.Trim().ToLower() == serialNumber.Trim().ToLower()).FirstOrDefault(); }
        public Scanner? GetScannerByModelNumber(string modelNumber) { return _scanners.Where(x => x.ModelNumber?.Trim().ToLower() == modelNumber.Trim().ToLower()).FirstOrDefault(); }

        //Scanner Commands
        //Enable/Disable
        public void SetEnabledScanner(Scanner scan, bool enabled) { scan.SetEnabledInternal(enabled); _scannerLink.SetEnabledScanner(scan, enabled); InternalUpdateLEDEvent(); }
        public void SetEnabledAllScanners(bool enabled) { foreach (Scanner scan in _scanners) { scan.SetEnabledInternal(enabled); _scannerLink.SetEnabledScanner(scan, enabled); } InternalUpdateLEDEvent(); }

        //Set LED Color
        public void SetLEDColorScanner(Scanner scan, LEDColor col) { _scannerLink.SetLEDColor(scan, col); }
        public void SetLEDColorAllScanners(LEDColor col) { foreach (Scanner scan in _scanners) _scannerLink.SetLEDColor(scan, col); }

        //Play Beep
        public void PlayBeepScanner(Scanner scan, BeepType beepType) { _scannerLink.BeepScanner(scan, beepType); }
        public void PlayBeepAllScanners(BeepType beepType) { foreach (Scanner scan in _scanners) _scannerLink.BeepScanner(scan, beepType); }

        //Pull Trigger
        public void PullTriggerScanner(Scanner scan) { _scannerLink.PullTriggerScanner(scan); }
        public void PullALlTriggerScanner() { foreach (Scanner scan in _scanners) _scannerLink.PullTriggerScanner(scan); }

        //Release Trigger
        public void ReleaseTriggerScanner(Scanner scan) { _scannerLink.ReleaseTriggerScanner(scan); }
        public void ReleaseTriggerAllScanners() { foreach (Scanner scan in _scanners) _scannerLink.ReleaseTriggerScanner(scan); }

        //Reboot 
        public void RebootScanner(Scanner scan) { _scannerLink.RebootScanner(scan); }
        public void RebootAllScanners() { foreach (Scanner scan in _scanners) _scannerLink.RebootScanner(scan); }

        //Aim On
        public void AimOnScanner(Scanner scan) { _scannerLink.AimOnScanner(scan); }
        public void AimOnAllScanners() { foreach (Scanner scan in _scanners) _scannerLink.AimOnScanner(scan); }

        //Aim Off
        public void AimOffScanner(Scanner scan) { _scannerLink.AimOffScanner(scan); }
        public void AimOffAllScanners() { foreach (Scanner scan in _scanners) _scannerLink.AimOffScanner(scan); }

        //Scale Zero
        public void ScaleZeroScanner(Scanner scan) { _scannerLink.ScaleZeroScanner(scan); }
        public void ScaleZeroAllScanners() { foreach (Scanner scan in _scanners) _scannerLink.ScaleZeroScanner(scan); }

        //Scale Reset
        public void ScaleResetScanner(Scanner scan) { _scannerLink.ScaleResetScanner(scan); }
        public void ScaleResetAllScanners() { foreach (Scanner scan in _scanners) _scannerLink.ScaleResetScanner(scan); }

        //Barcode Callback Registration and Unregister
        public void RegisterBarcodeCallback(Action<ScanData> NewCallback)
        {
            if(NewCallback != null) 
            {
                _barcodeScanCallbacks.Add(NewCallback);
            }
        }

        public void UnregisterBarcodeCallback(Action<ScanData> CallbackToUnregister) 
        {
            if (CallbackToUnregister != null)
            { 
                _barcodeScanCallbacks.Remove(CallbackToUnregister);
            }
        }


        //Weight Callback Registration and Unregister
        public void RegisterWeightCallback(Action<WeightData> NewCallback) 
        {
            if (NewCallback != null) 
            {
                _weightDataCallbacks.Add(NewCallback);
            }
        }

        public void UnregisterWeightCallback(Action<WeightData> CallbackToUnregister)
        {
            if (CallbackToUnregister != null)
            {
                _weightDataCallbacks.Remove(CallbackToUnregister);
            }
        }


        //Plug and Play Callback Registration and Unregister
        public void RegisterPNPCallback(Action<PNPData> NewCallback)
        {
            if (NewCallback != null)
            {
                _pnpDataCallbacks.Add(NewCallback);
            }
        }

        public void UnregisterPNPCallback(Action<PNPData> CallbackToUnregister)
        {
            if (CallbackToUnregister != null)
            {
                _pnpDataCallbacks.Remove(CallbackToUnregister);
            }
        }




    }
}
