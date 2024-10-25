using CoreScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraScannerWrapper
{
    internal class CoreScannerLink
    {
        private CCoreScanner Cscanner = new CCoreScanner();

        internal int ConfigureCoreScanner(Array ScannerTypes) 
        {
            Cscanner.Open(0, ScannerTypes, (short)ScannerTypes.Length, out int status);
            return status;
        }

        internal void RegisterEvents(Action<short, string> pnp, Action<short, string> scan) 
        {
            Cscanner.PNPEvent += (short evntType, ref string pnpData) => { pnp(evntType, pnpData); };
            Cscanner.BarcodeEvent += (short evntType, ref string scanData) => { pnp(evntType, scanData); };

            string inXML = @$"<inArgs>
                                <cmdArgs>
                                    <arg-int>4</arg-int>
                                    <arg-int>1,8,16,32</arg-int>
                                </cmdArgs>
                             </inArgs>";

            Execute(inXML, 1001, out string outXML, out int status);
        }

        internal void Execute(string inXml, int opcode, out string outXML, out int status)
        {
            Cscanner.ExecCommand(opcode, inXml, out outXML, out status);
        }

    }
}
