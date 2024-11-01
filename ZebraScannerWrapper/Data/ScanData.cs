using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebraScannerWrapper.Enums;

namespace ZebraScannerWrapper.Data
{
    public class ScanData
    {
        public Scanner Scanner { get; set; }
        public BarcodeType BarcodeType { get; set; }
        public string Barcode { get; set; } = "";
        public byte[] RawBarcodeData { get; set; } = new byte[1];

        public ScanData(Scanner Scan)
        {
            Scanner = Scan;
        }
    }
}
