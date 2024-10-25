using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraScannerWrapper.Enums
{
    public enum ScannerTypes : short
    {
        SCANNER_TYPES_ALL = 1,
        SCANNER_TYPES_SNAPI = 2,
        SCANNER_TYPES_SSI = 3,
        SCANNER_TYPES_IBMHID = 4,
        SCANNER_TYPES_NIXMODB = 7,
        SCANNER_TYPES_HIDKB = 8,
        SCANNER_TYPES_IBMTT = 9,
    }
}
