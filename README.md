# Zebra Scanner Wrapper For C# .NET CORE 8

This C# class libary is built around the zebra core scanner driver which is avaliable as a COM object from: https://techdocs.zebra.com/dcs/scanners/sdk-windows/about/ 

Demonstration app:
![image](https://github.com/user-attachments/assets/1335280d-cdf4-4d63-b508-75e3e1b6ab80)


### This wrapper provides the following:
- Object abstractions of the xml based C API.
- Custom LED mangement indicating whether a scanner is enabled or disabled.
- Centralised management of all connected scanners included nested scanners.
- Live scale weight support for scales (implemented with polling).
- Event based callbacks for PNP, barcodes, scale weight.

### Supported Scanner Commands:
- Enable / Disable
- SetLEDColor
- PlayBeep
- Pull / Release Trigger
- Reboot Scanner
- Aim On / Off
- Scale Zero
- Scale Reset
- Scale Weight

### Tested with:
- Zebra MP7001
- Zebra DS4608
- Zebra MS4717

### Basic Setup:
```csharp
//Create the manager, this will need to be accessed wherever you need to call commands to the manager or scanners
_manager = new ScannerManager(ScannerManager.DefaultSupportedScannerTypes, true, true);

//Register any events that you would like to recieve, you can add many different callbacks for different areas of your application.
//can be removed by calling UnRegister*CallbackName*(function)
_manager.RegisterPNPCallback(RecievePnp);                             //RecievePnp(PNPData pnpData)
_manager.RegisterBarcodeCallback(RecieveScan);                        //RecieveScan(ScanData scanData) 
_manager.RegisterWeightCallback(RecieveWeightLive);                   //RecieveWeightLive(WeightData weight)
_manager.RegisterLiveWeightStatusCallback(RecieveLiveWeightStatus);   //RecieveLiveWeightStatus(bool enabled)

```

> [!IMPORTANT]
> Events will only report the parent scanner if using a nested scanner setup. e.g. MP7001 -> DS4608 if the DS was the one to scan the Core Scanner API will return MP7001.

### Using Scanners
```csharp
  Scanner scan = _manager.GetScannerBySerialNumber("*SerialNumber*");
  //OR
  Scanner scan = _manager.GetScannerByModelNumber("*ModelNumber*");
  //Or get one from the list of scanners by calling _manager.GetScanners();

  ScannerResponse resp = scan.SetEnabled(true);
  //OR
  ScannerResponse resp = _manager.SetEnabledScanner(scan, true);
  //OR for all scanners
  List<ScannerResponse> responses = _manager.SetEnabledAllScanners(true);
```
### Checking Scanner Responses
```csharp
  //all responses can be found at https://techdocs.zebra.com/dcs/scanners/sdk-windows/appendix/ 
  if(resp.Response == ScannerStatus.SUCCESS)
  {
    
  }
  //resp.Scanner provides the scanner this response came from if going through a list of responses. 
```

### Get Scale Weight
```csharp
  ScannerResponse response = scan.GetScaleWeight();
    if (response.Response == ScannerStatus.SUCCESS)
    {
      WeightData? data = (WeightData?)response.ResponseData;
      if (data != null)
      {
        Console.WriteLine(data.Weight + data.WeightUnit.ToString());
      }
    }
```

### Command List
| Opcode | Command on Scanner | Command on Manager | Command for All |
| ------ | ------------------ | ------------------ | --------------- |
| 6000 | `SetLEDColor(LEDColor col)` | `SetLEDColorScanner(Scanner scan, LEDColor col)` | `SetLEDColorAllScanners(LEDColor col)` |
| 6000 | `PlayBeep(BeepType beepType)` | `PlayBeepScanner(Scanner scan, BeepType beepType)` | `PlayBeepAllScanners(BeepType beepType)` |
| 2014 / 2013 | `SetEnabled(bool enabled)` | `SetEnabledScanner(Scanner scan, bool enabled)` | `SetEnabledAllScanners(bool enabled)` |
| 2011 | `PullTrigger()` | `PullTriggerScanner(Scanner scan)` | `PullAllTriggerScanner()` |
| 2012 | `ReleaseTrigger()` | `ReleaseTriggerScanner(Scanner scan)` | `ReleaseTriggerAllScanners()` |
| 2019 | `Reboot()` | `RebootScanner(Scanner scan)` | `RebootAllScanners()` |
| 2003 | `AimOn()` | `AimOnScanner(Scanner scan)` | `AimOnAllScanners()` |
| 2002 | `AimOff()` | `AimOffScanner(Scanner scan)` | `AimOffAllScanners()` |
| 7002 | `ScaleZero()` | `ScaleZeroScanner(Scanner scan)` | `ScaleZeroAllScanners()` |
| 7015 | `ScaleReset()` | `ScaleResetScanner(Scanner scan)` | `ScaleResetAllScanners()` |
| 7000 | `GetScaleWeight()` | `GetScaleWeightScanner(Scanner scan)` | `GetScaleWeightAllScanners()` |
