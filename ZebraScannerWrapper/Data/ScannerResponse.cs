using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebraScannerWrapper.Enums;

namespace ZebraScannerWrapper.Data
{
    public class ScannerResponse
    {
        public Scanner? RespondingScanner;
        public ScannerStatus Response;
        public object? ResponseData;

        public ScannerResponse(Scanner Scan, ScannerStatus Stat, object? Data = null)
        {
            RespondingScanner = Scan;
            Response = Stat;
            ResponseData = Data;
        }
    }
}
