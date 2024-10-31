using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebraScannerWrapper.Enums;

namespace ZebraScannerWrapper.Data
{
    public class PNPData
    {
        public Scanner Scanner { get; set; }
        public PNPEventType PNPEvent { get; set; }
    }
}
