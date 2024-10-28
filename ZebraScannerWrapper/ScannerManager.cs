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
        public ScannerResponse SetEnabledScanner(Scanner scan, bool enabled) 
        { 
            scan.SetEnabledInternal(enabled); 
            var response =  _scannerLink.SetEnabledScanner(scan, enabled);
            InternalUpdateLEDEvent();
            return response;
        }

        public List<ScannerResponse> SetEnabledAllScanners(bool enabled) 
        { 
            List<ScannerResponse> responses = new List<ScannerResponse>();
            foreach (Scanner scan in _scanners) 
            { 
                scan.SetEnabledInternal(enabled); 
                responses.Add(_scannerLink.SetEnabledScanner(scan, enabled)); 
            } 
            InternalUpdateLEDEvent(); 
            return responses;
        }

        //Set LED Color
        public ScannerResponse SetLEDColorScanner(Scanner scan, LEDColor col) { return _scannerLink.SetLEDColor(scan, col); }
        public List<ScannerResponse> SetLEDColorAllScanners(LEDColor col) 
        {
            List<ScannerResponse> responses = new List<ScannerResponse>();
            foreach (Scanner scan in _scanners) responses.Add(_scannerLink.SetLEDColor(scan, col));
            return responses;
        }

        //Play Beep
        public ScannerResponse PlayBeepScanner(Scanner scan, BeepType beepType) { return _scannerLink.BeepScanner(scan, beepType); }
        public List<ScannerResponse> PlayBeepAllScanners(BeepType beepType) 
        {
            List<ScannerResponse> responses = new List<ScannerResponse>();
            foreach (Scanner scan in _scanners) responses.Add(_scannerLink.BeepScanner(scan, beepType));
            return responses;
        }

        //Pull Trigger
        public ScannerResponse PullTriggerScanner(Scanner scan) { return _scannerLink.PullTriggerScanner(scan); }
        public List<ScannerResponse> PullAllTriggerScanner() 
        {
            List<ScannerResponse> responses = new List<ScannerResponse>();
            foreach (Scanner scan in _scanners) responses.Add(_scannerLink.PullTriggerScanner(scan)); 
            return responses;
        }

        //Release Trigger
        public ScannerResponse ReleaseTriggerScanner(Scanner scan) { return _scannerLink.ReleaseTriggerScanner(scan); }
        public List<ScannerResponse> ReleaseTriggerAllScanners() {
            List<ScannerResponse> responses = new List<ScannerResponse>();
            foreach (Scanner scan in _scanners) responses.Add(_scannerLink.ReleaseTriggerScanner(scan));
            return responses;
        }

        //Reboot 
        public ScannerResponse RebootScanner(Scanner scan) { return _scannerLink.RebootScanner(scan); }
        public List<ScannerResponse> RebootAllScanners() 
        {
            List<ScannerResponse> responses = new List<ScannerResponse>();
            foreach (Scanner scan in _scanners) responses.Add(_scannerLink.RebootScanner(scan));
            return responses;
        }

        //Aim On
        public ScannerResponse AimOnScanner(Scanner scan) { return _scannerLink.AimOnScanner(scan); }
        public List<ScannerResponse> AimOnAllScanners() 
        {
            List<ScannerResponse> responses = new List<ScannerResponse>();
            foreach (Scanner scan in _scanners) responses.Add(_scannerLink.AimOnScanner(scan));
            return responses;
        }

        //Aim Off
        public ScannerResponse AimOffScanner(Scanner scan) { return _scannerLink.AimOffScanner(scan); }
        public List<ScannerResponse> AimOffAllScanners()
        {
            List<ScannerResponse> responses = new List<ScannerResponse>();
            foreach (Scanner scan in _scanners) responses.Add(_scannerLink.AimOffScanner(scan)); 
            return responses;
        }

        //Scale Zero
        public ScannerResponse ScaleZeroScanner(Scanner scan) { return _scannerLink.ScaleZeroScanner(scan); }
        public List<ScannerResponse> ScaleZeroAllScanners() 
        {
            List<ScannerResponse> responses = new List<ScannerResponse>();
            foreach (Scanner scan in _scanners) responses.Add(_scannerLink.ScaleZeroScanner(scan));
            return responses;
        }

        //Scale Reset
        public ScannerResponse ScaleResetScanner(Scanner scan) { return _scannerLink.ScaleResetScanner(scan); }
        public List<ScannerResponse> ScaleResetAllScanners() 
        {
            List<ScannerResponse> responses = new List<ScannerResponse>();
            foreach (Scanner scan in _scanners) responses.Add(_scannerLink.ScaleResetScanner(scan)); 
            return responses;
        }

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
