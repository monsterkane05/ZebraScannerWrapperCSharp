using System.Net;

namespace ZebraScannerWrapper
{
    public class ScannerManager
    {
        //Callback Definitions
        private List<Action<ScanData>> _barcodeScanCallbacks;
        private List<Action<WeightData>> _weightDataCallbacks;
        private List<Action<PNPData>> _pnpDataCallbacks;



        

    }
}
