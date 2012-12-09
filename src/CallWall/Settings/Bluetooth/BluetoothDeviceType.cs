using System;
using InTheHand.Net.Bluetooth;

namespace CallWall.Settings.Bluetooth
{
    public sealed class BluetoothDeviceType
    {
        #region Static fields and constructor

        private static readonly Uri _audioVisualImageUri;
        private static readonly Uri _phoneImageUri;
        private static readonly Uri _computerImageUri;
        private static readonly Uri _networkImageUri;
        private static readonly Uri _peripheralImageUri;
        private static readonly Uri _imagingImageUri;
        private static readonly Uri _miscellaneousImageUri;

        static BluetoothDeviceType()
        {
            if (!UriParser.IsKnownScheme("pack"))
            {
                UriParser.Register(new GenericUriParser(GenericUriParserOptions.GenericAuthority), "pack", -1);
            }
            _phoneImageUri = new Uri("pack://application:,,,/CallWall;component/Images/Bluetooth/Phone.png");
            _audioVisualImageUri = new Uri("pack://application:,,,/CallWall;component/Images/AudioVisual.png");
            _computerImageUri = new Uri("pack://application:,,,/CallWall;component/Images/Bluetooth/Computer.png");
            _networkImageUri = new Uri("pack://application:,,,/CallWall;component/Images/Bluetooth/Network.png");
            _peripheralImageUri = new Uri("pack://application:,,,/CallWall;component/Images/Bluetooth/Peripheral.png");
            _imagingImageUri = new Uri("pack://application:,,,/CallWall;component/Images/Bluetooth/Imaging.png");
            //_wearableImageUri = new Uri("pack://application:,,,/CallWall;component/Images/Wearable.png");
            //_toyImageUri = new Uri("pack://application:,,,/CallWall;component/Images/Toy.png");
            //_medicalImageUri = new Uri("pack://application:,,,/CallWall;component/Images/Medical.png");
            _miscellaneousImageUri = new Uri("pack://application:,,,/CallWall;component/Images/Bluetooth/Miscellaneous.png");
        }

        #endregion

        private readonly string _name;
        private readonly Uri _image;
        private readonly bool _isValid;

        private BluetoothDeviceType(string name, Uri image, bool isValid)
        {
            _name = name;
            _image = image;
            _isValid = isValid;
        }

        public bool IsValid { get { return _isValid; } }
        public string Name { get { return _name; } }
        public Uri Image { get { return _image; } }

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
                    return new BluetoothDeviceType(deviceClass.ToString(), _phoneImageUri, true);

                //Computers
                case DeviceClass.Computer:
                case DeviceClass.DesktopComputer:
                case DeviceClass.ServerComputer:
                case DeviceClass.LaptopComputer:
                case DeviceClass.HandheldComputer:
                case DeviceClass.PdaComputer:
                case DeviceClass.WearableComputer:
                    return new BluetoothDeviceType(deviceClass.ToString(), _computerImageUri, false);
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
                    return new BluetoothDeviceType(deviceClass.ToString(), _networkImageUri, false);
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
                    return new BluetoothDeviceType(deviceClass.ToString(), _audioVisualImageUri, false);
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
                    return new BluetoothDeviceType(deviceClass.ToString(), _peripheralImageUri, false);
                //Imaging/Printers/Cameras
                case DeviceClass.Imaging:
                case DeviceClass.ImagingDisplay:
                case DeviceClass.ImagingCamera:
                case DeviceClass.ImagingScanner:
                case DeviceClass.ImagingPrinter:
                    return new BluetoothDeviceType(deviceClass.ToString(), _imagingImageUri, false);
                //Wearable
                case DeviceClass.Wearable:
                case DeviceClass.WearableWristWatch:
                case DeviceClass.WearablePager:
                case DeviceClass.WearableJacket:
                case DeviceClass.WearableHelmet:
                case DeviceClass.WearableGlasses:
                //Toys
                case DeviceClass.Toy:
                case DeviceClass.ToyRobot:
                case DeviceClass.ToyVehicle:
                case DeviceClass.ToyFigure:
                case DeviceClass.ToyController:
                case DeviceClass.ToyGame:
                //Medical
                case DeviceClass.Medical:
                case DeviceClass.MedicalBloodPressureMonitor:
                case DeviceClass.MedicalThermometer:
                case DeviceClass.MedicalWeighingScale:
                case DeviceClass.MedicalGlucoseMeter:
                case DeviceClass.MedicalPulseOximeter:
                case DeviceClass.MedicalHeartPulseRateMonitor:
                case DeviceClass.MedicalDataDisplay:
                //Other/Unknown
                case DeviceClass.Uncategorized:
                case DeviceClass.Miscellaneous:
                default:
                    return new BluetoothDeviceType(deviceClass.ToString(), _miscellaneousImageUri, false);
            }
        }
    }
}