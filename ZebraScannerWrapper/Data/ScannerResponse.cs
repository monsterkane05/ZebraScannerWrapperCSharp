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


        public ScannerResponse(Scanner Scan, ScannerStatus Stat)
        {
            RespondingScanner = Scan;
            Response = Stat;
        }
    }
}
