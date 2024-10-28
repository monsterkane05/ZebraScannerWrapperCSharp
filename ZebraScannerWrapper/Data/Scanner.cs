using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebraScannerWrapper.Enums;

namespace ZebraScannerWrapper.Data
{
    public class Scanner
    {
        private int ID { get; set; }

        public ScannerType ScannerType { get; private set; }
        public string? SerialNumber { get; private set; }
        public string? GUID { get; private set; }
        public string? VID { get; private set; }
        public string? PID { get; private set; }
        public string? ModelNumber { get; private set; }
        public DateTime? DateOfManafacture { get; private set; }
        public string? FirmwareVersion { get; private set; }

        private bool Enabled { get; set; } = true;

        private ScannerManager _manager;

        public Scanner(ScannerManager manager)
        {
            _manager = manager;   
        }

        //Internal Setters
        internal void SetID(int Id) { ID = Id; }

        internal int GetID() { return ID; }

        internal void SetScanerType(ScannerType type) { ScannerType = type; }

        internal void SetSerialNumber(string? NewSerialNumber) { SerialNumber = NewSerialNumber; }

        internal void SetGUID(string? NewGUID) { GUID = NewGUID; }

        internal void SetVID(string? NewVID) { VID = NewVID; }

        internal void SetPID(string? NewPID) {  PID = NewPID; }

        internal void SetModelNumber(string? NewModelNumber) { ModelNumber = NewModelNumber; }

        internal void SetDateOfManafacture(DateTime? NewDateOfManafacture) { DateOfManafacture = NewDateOfManafacture; }

        internal void SetFirmwareVersion(string? NewFirmwareVersion) { FirmwareVersion = NewFirmwareVersion; }

        internal void SetEnabledInternal(bool NewEnabled) { Enabled = NewEnabled; }

        internal bool GetEnabled() { return Enabled; }


        //public scanner API
        public void SetLEDColor(LEDColor col) { _manager.SetLEDColorScanner(this, col); }
        public void SetEnabled(bool enabled) { _manager.SetEnabledScanner(this, enabled); }
        public void PlayBeep(BeepType beepType) { _manager.PlayBeepScanner(this, beepType); }
        public void PullTrigger() {  _manager.PullTriggerScanner(this); }
        public void ReleaseTrigger() { _manager.ReleaseTriggerScanner(this); }
        public void Reboot() { _manager.RebootScanner(this); }
        public void AimOn() {  _manager.AimOnScanner(this); }
        public void AimOff() { _manager.AimOffScanner(this); }
        public void ScaleZero() { _manager.ScaleZeroScanner(this); }
        public void ScaleReset() { _manager.ScaleResetScanner(this); }
            }
}
