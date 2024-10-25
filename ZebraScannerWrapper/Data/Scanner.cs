using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraScannerWrapper.Data
{
    public class Scanner
    {
        private int ID { get; set; }
        public string? SerialNumber { get; set; }
        public string? GUID { get; set; }
        public string? VID { get; set; }
        public string? PID { get; set; }
        public string? ModelNumber { get; set; }
        public string? DateOfManafacture { get; set; }
        public string? Firmware { get; set; }

        public bool Enabled { get; set; }
    }
}
