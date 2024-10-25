using System.Net;
using ZebraScannerWrapper.Data;
using ZebraScannerWrapper.Enums;

namespace ZebraScannerWrapper
{
    public class ScannerManager
    {
        //Static Configuration Array
        public static List<ScannerTypes> DefaultSupportedScannerTypes = new List<ScannerTypes>() { ScannerTypes.SCANNER_TYPES_ALL };


        //Callback Definitions
        private List<Action<ScanData>> _barcodeScanCallbacks = new List<Action<ScanData>>();
        private List<Action<WeightData>> _weightDataCallbacks = new List<Action<WeightData>>();
        private List<Action<PNPData>> _pnpDataCallbacks = new List<Action<PNPData>>();

        private List<Scanner> _scanners = new List<Scanner>();

        //Management Properties
        private bool _customLEDManagementEnabled;
        private bool _liveWeightSubsystemEnabled;

        private CoreScannerLink _scannerLink = new CoreScannerLink();

        //Constructor
        public ScannerManager(List<ScannerTypes> SupportedScannerTypes, bool EnableCustomLEDManagement, bool EnableLiveWeightSubsystem)
        {
            _customLEDManagementEnabled = EnableCustomLEDManagement;
            _liveWeightSubsystemEnabled = EnableLiveWeightSubsystem;

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
