using System;
using CallWall.Windows.Connectivity.Images;
using InTheHand.Net.Bluetooth;

namespace CallWall.Windows.Connectivity.Settings.Bluetooth
{
    public sealed class BluetoothDeviceType
    {
        private readonly string _name;
        private readonly Uri _image;
        private readonly bool _isValid;
        private readonly byte _ordinal;

        private BluetoothDeviceType(string name, Uri image, bool isValid, byte ordinal)
        {
            _name = name;
            _image = image;
            _isValid = isValid;
            _ordinal = ordinal;
        }

        public bool IsValid { get { return _isValid; } }
        public string Name { get { return _name; } }
        public Uri Image { get { return _image; } }
        public byte Ordinal { get { return _ordinal; } }

        public static BluetoothDeviceType Create(DeviceClass deviceClass)
        {
            switch (deviceClass)
            {
                //Phones
                case DeviceClass.Phone:
                case DeviceClass.CellPhone:
                case DeviceClass.CordlessPhone:
                case DeviceClass.SmartPhone:
                case DeviceClass.WiredPhone:
                    return new BluetoothDeviceType(deviceClass.ToString(), BluetoothImages.PhoneIconUri, true, 1);
                //Computers
                case DeviceClass.Computer:
                case DeviceClass.DesktopComputer:
                case DeviceClass.ServerComputer:
                case DeviceClass.LaptopComputer:
                case DeviceClass.HandheldComputer:
                case DeviceClass.PdaComputer:
                case DeviceClass.WearableComputer:
                    return new BluetoothDeviceType(deviceClass.ToString(), BluetoothImages.ComputerIconUri, false, 2);
                //Network
                case DeviceClass.IsdnAccess:
                case DeviceClass.AccessPointAvailable:
                case DeviceClass.AccessPoint1To17:
                case DeviceClass.AccessPoint17To33:
                case DeviceClass.AccessPoint33To50:
                case DeviceClass.AccessPoint50To67:
                case DeviceClass.AccessPoint67To83:
                case DeviceClass.AccessPoint83To99:
                case DeviceClass.AccessPointNoService:
                    return new BluetoothDeviceType(deviceClass.ToString(), BluetoothImages.NetworkIconUri, false, 3);
                //AV
                case DeviceClass.AudioVideoUnclassified:
                case DeviceClass.AudioVideoHeadset:
                case DeviceClass.AudioVideoHandsFree:
                case DeviceClass.AudioVideoMicrophone:
                case DeviceClass.AudioVideoLoudSpeaker:
                case DeviceClass.AudioVideoHeadphones:
                case DeviceClass.AudioVideoPortable:
                case DeviceClass.AudioVideoCar:
                case DeviceClass.AudioVideoSetTopBox:
                case DeviceClass.AudioVideoHiFi:
                case DeviceClass.AudioVideoVcr:
                case DeviceClass.AudioVideoVideoCamera:
                case DeviceClass.AudioVideoCamcorder:
                case DeviceClass.AudioVideoMonitor:
                case DeviceClass.AudioVideoDisplayLoudSpeaker:
                case DeviceClass.AudioVideoVideoConferencing:
                case DeviceClass.AudioVideoGaming:
                    return new BluetoothDeviceType(deviceClass.ToString(), BluetoothImages.AudioVisualIconUri, false, 3);
                //Peripheral/Accessories
                case DeviceClass.Peripheral:
                case DeviceClass.PeripheralJoystick:
                case DeviceClass.PeripheralGamepad:
                case DeviceClass.PeripheralRemoteControl:
                case DeviceClass.PeripheralSensingDevice:
                case DeviceClass.PeripheralDigitizerTablet:
                case DeviceClass.PeripheralCardReader:
                case DeviceClass.PeripheralKeyboard:
                case DeviceClass.PeripheralPointingDevice:
                case DeviceClass.PeripheralCombinedKeyboardPointingDevice:
                    return new BluetoothDeviceType(deviceClass.ToString(), BluetoothImages.PeripheralIconUri, false, 3);
                //Imaging/Printers/Cameras
                case DeviceClass.Imaging:
                case DeviceClass.ImagingDisplay:
                case DeviceClass.ImagingCamera:
                case DeviceClass.ImagingScanner:
                case DeviceClass.ImagingPrinter:
                    return new BluetoothDeviceType(deviceClass.ToString(), BluetoothImages.ImagingIconUri, false, 3);
                ////Wearable
                //case DeviceClass.Wearable:
                //case DeviceClass.WearableWristWatch:
                //case DeviceClass.WearablePager:
                //case DeviceClass.WearableJacket:
                //case DeviceClass.WearableHelmet:
                //case DeviceClass.WearableGlasses:
                ////Toys
                //case DeviceClass.Toy:
                //case DeviceClass.ToyRobot:
                //case DeviceClass.ToyVehicle:
                //case DeviceClass.ToyFigure:
                //case DeviceClass.ToyController:
                //case DeviceClass.ToyGame:
                ////Medical
                //case DeviceClass.Medical:
                //case DeviceClass.MedicalBloodPressureMonitor:
                //case DeviceClass.MedicalThermometer:
                //case DeviceClass.MedicalWeighingScale:
                //case DeviceClass.MedicalGlucoseMeter:
                //case DeviceClass.MedicalPulseOximeter:
                //case DeviceClass.MedicalHeartPulseRateMonitor:
                //case DeviceClass.MedicalDataDisplay:
                ////Other/Unknown
                //case DeviceClass.Uncategorized:
                //case DeviceClass.Miscellaneous:
                default:
                    return new BluetoothDeviceType(deviceClass.ToString(), BluetoothImages.MiscellaneousIconUri, false, 3);
            }
        }
    }
}