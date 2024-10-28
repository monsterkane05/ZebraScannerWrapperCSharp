// See https://aka.ms/new-console-template for more information
using ZebraScannerWrapper;

Console.WriteLine("Attempting to open core scanner");

ScannerManager manager = new ScannerManager(ScannerManager.DefaultSupportedScannerTypes, true, false);



while (true) 
{
    Thread.Sleep(2000);
    manager.GetScannerBySerialNumber("21006010550437").SetEnabled(true);
    manager.GetScannerBySerialNumber("21006010550437").PullTrigger();
    Thread.Sleep(2000);
    manager.GetScannerBySerialNumber("21006010550437").SetEnabled(false);
    manager.GetScannerBySerialNumber("21006010550437").PlayBeep(ZebraScannerWrapper.Enums.BeepType.ONE_HIGH_SHORT_BEEP);
    
}