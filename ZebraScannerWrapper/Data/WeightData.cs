using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebraScannerWrapper.Enums;

namespace ZebraScannerWrapper.Data
{
    public class WeightData
    {
        public Scanner Scanner { get; set; }
        public decimal Weight { get; set; }
        public WeightUnit WeightUnit { get; set; }
        public WeightStatus WeightStatus { get; set; }


        public WeightData(Scanner Scan)
        {
            Scanner = Scan;
        }
    }
}
