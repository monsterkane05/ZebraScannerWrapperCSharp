using CoreScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZebraScannerWrapper.Data;
using ZebraScannerWrapper.Enums;

namespace ZebraScannerWrapper
{
    internal class CoreScannerLink
    {
        private CCoreScanner _cScanner = new CCoreScanner();
        private ScannerManager _manager;

        public CoreScannerLink(ScannerManager manager)
        {
            _manager = manager;
        }


        internal int ConfigureCoreScanner(Array ScannerTypes) 
        {
            _cScanner.Open(0, ScannerTypes, (short)ScannerTypes.Length, out int status);
            return status;
        }

        internal void RegisterEvents(Action<short, string> pnp, Action<short, string> scan) 
        {
            _cScanner.PNPEvent += (short evntType, ref string pnpData) => { pnp(evntType, pnpData); };
            _cScanner.BarcodeEvent += (short evntType, ref string scanData) => { scan(evntType, scanData); };

            string inXML = @$"<inArgs>
                                <cmdArgs>
                                    <arg-int>4</arg-int>
                                    <arg-int>1,8,16,32</arg-int>
                                </cmdArgs>
                             </inArgs>";

            Execute(inXML, 1001, out string outXML, out int status);
        }

        internal List<Scanner> GetScannerList()
        {
            int[] scanneridList = new int[255];
            _cScanner.GetScanners(out short numberOfScanners, scanneridList, out string outXML, out int status);
            List<Scanner> scannerList = new List<Scanner>();


            if (status == 0) 
            {
                XElement scanXMLElements = XElement.Parse(outXML);
                List<XElement> scanXMLNodes = scanXMLElements.Elements("scanner").ToList();

                foreach (XElement scanXMLElement in scanXMLNodes)
                {
                    Scanner newScanner = new Scanner(_manager);

                    string? type = scanXMLElement.Attribute("type")?.Value;
                    if (type != null) 
                    {
                        if (Enum.TryParse(typeof(ScannerType), type, out object? typeEnum))
                        {
                            newScanner.SetScanerType((ScannerType)typeEnum);
                        }
                    }

                    if (scanXMLElement.Element("scannerID") != null) 
                    {
                        if(int.TryParse(scanXMLElement.Element("scannerID")?.Value, out int id)) 
                        {
                            newScanner.SetID(id);
                        }
                    }

                    if (scanXMLElement.Element("serialnumber") != null)
                    {
                        newScanner.SetSerialNumber(scanXMLElement.Element("serialnumber")?.Value);
                    }

                    if (scanXMLElement.Element("GUID") != null)
                    {
                        newScanner.SetGUID(scanXMLElement.Element("GUID")?.Value);
                    }

                    if (scanXMLElement.Element("VID") != null)
                    {
                        newScanner.SetVID(scanXMLElement.Element("VID")?.Value);
                    }

                    if (scanXMLElement.Element("PID") != null)
                    {
                        newScanner.SetPID(scanXMLElement.Element("PID")?.Value);
                    }

                    if (scanXMLElement.Element("modelnumber") != null)
                    {
                        newScanner.SetModelNumber(scanXMLElement.Element("modelnumber")?.Value);
                    }

                    if (scanXMLElement.Element("DoM") != null)
                    {
                        if(DateTime.TryParse(scanXMLElement.Element("DoM")?.Value, out DateTime res)) 
                        {
                            newScanner.SetDateOfManafacture(res);
                        }
                        else { newScanner.SetDateOfManafacture(null); }
                    }

                    if (scanXMLElement.Element("firmware") != null)
                    {
                        newScanner.SetFirmwareVersion(scanXMLElement.Element("firmware")?.Value);
                    }
                    scannerList.Add(newScanner);
                }
            }
            else 
            {
                throw new Exception();
            }


            return scannerList;
        }

        internal ScannerResponse SetEnabledScanner(Scanner scanner, bool enabled)
        {
            if (enabled) 
            {
                string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                            </inArgs>";

                Execute(inXML, 2014, out string xml, out int stat);
                return new ScannerResponse(scanner, (ScannerStatus) stat);
            }
            else
            {
                string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                            </inArgs>";

                Execute(inXML, 2013, out string xml, out int stat);
                return new ScannerResponse(scanner, (ScannerStatus)stat);
            }
        }


        internal ScannerResponse SetLEDColor(Scanner scanner, LEDColor col) 
        {
            string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                                <cmdArgs>
                                    <arg-int>{(int)col}</arg-int>
                                </cmdArgs>
                            </inArgs>";

            Execute(inXML, 6000, out string xml, out int stat);
            return new ScannerResponse(scanner, (ScannerStatus)stat);
        }

        internal ScannerResponse BeepScanner(Scanner scanner, BeepType beepType) 
        {
            string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                                <cmdArgs>
                                    <arg-int>{(int)beepType}</arg-int>
                                </cmdArgs>
                            </inArgs>";

            Execute(inXML, 6000, out string xml, out int stat);
            return new ScannerResponse(scanner, (ScannerStatus)stat);
        }

        internal ScannerResponse PullTriggerScanner(Scanner scanner) 
        {
            string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                            </inArgs>";

            Execute(inXML, 2011, out string xml, out int stat);
            return new ScannerResponse(scanner, (ScannerStatus)stat);
        }

        internal ScannerResponse ReleaseTriggerScanner(Scanner scanner)
        {
            string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                            </inArgs>";

            Execute(inXML, 2012, out string xml, out int stat);
            return new ScannerResponse(scanner, (ScannerStatus)stat);
        }

        internal ScannerResponse RebootScanner(Scanner scanner)
        {
            string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                            </inArgs>";

            Execute(inXML, 2019, out string xml, out int stat);
            return new ScannerResponse(scanner, (ScannerStatus)stat);
        }

        internal ScannerResponse AimOnScanner(Scanner scanner)
        {
            string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                            </inArgs>";

            Execute(inXML, 2003, out string xml, out int stat);
            return new ScannerResponse(scanner, (ScannerStatus)stat);
        }

        internal ScannerResponse AimOffScanner(Scanner scanner)
        {
            string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                            </inArgs>";

            Execute(inXML, 2002, out string xml, out int stat);
            return new ScannerResponse(scanner, (ScannerStatus)stat);
        }

        internal ScannerResponse ScaleZeroScanner(Scanner scanner)
        {
            string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                            </inArgs>";

            Execute(inXML, 7002, out string xml, out int stat);
            return new ScannerResponse(scanner, (ScannerStatus)stat);
        }

        internal ScannerResponse ScaleResetScanner(Scanner scanner)
        {
            string inXML = @$"<inArgs>
                                <scannerID>{scanner.GetID()}</scannerID>
                            </inArgs>";

            Execute(inXML, 7015, out string xml, out int stat);
            return new ScannerResponse(scanner, (ScannerStatus)stat);
        }

        internal void Execute(string inXml, int opcode, out string outXML, out int status)
        {
            _cScanner.ExecCommand(opcode, inXml, out outXML, out status);
        }
    }
}
