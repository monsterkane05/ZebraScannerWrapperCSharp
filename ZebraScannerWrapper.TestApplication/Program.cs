// See https://aka.ms/new-console-template for more information
using ZebraScannerWrapper;
using ZebraScannerWrapper.Data;

Console.WriteLine("Attempting to open core scanner");

ScannerManager manager = new ScannerManager(ScannerManager.DefaultSupportedScannerTypes, true, false);

manager.RegisterBarcodeCallback(x => { Console.WriteLine("BARCODE: " + x.Barcode + " TYPE: " + x.BarcodeType.ToString() + " SCANNER: " + x.Scanner.ModelNumber); });

manager.RegisterPNPCallback(x => { Console.WriteLine("SCANNER PNP: " + x.Scanner.ModelNumber + " EVENT: " + x.PNPEvent.ToString()); });


while (true) 
{
    Thread.Sleep(2000);

    Scanner? scan = manager.GetScannerBySerialNumber("21006010550437");


    if(scan != null) 
    {
        ScannerResponse resp = scan.SetEnabled(true);
        scan.PullTrigger();
    }
    
    Thread.Sleep(2000);

    if (scan != null) 
    {
        scan.SetEnabled(false);
        scan.PlayBeep(ZebraScannerWrapper.Enums.BeepType.ONE_HIGH_SHORT_BEEP); 
    }
    

    
}