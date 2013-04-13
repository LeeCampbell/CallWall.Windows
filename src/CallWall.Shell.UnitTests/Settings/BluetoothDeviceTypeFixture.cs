using System;
using CallWall.Settings.Connectivity.Bluetooth;
using CallWall.Testing;
using InTheHand.Net.Bluetooth;
using NUnit.Framework;


// ReSharper disable InconsistentNaming
namespace CallWall.UnitTests.Settings
{
    [TestFixture]
    public sealed class BluetoothDeviceTypeFixture
    {
        [Test]
        public void Should_return_IsValid_for_phone_types(
            [AllEnumValues(typeof(DeviceClass))] DeviceClass deviceClass)
        {
            var expected = false;

            switch (deviceClass)
            {
                case DeviceClass.Phone:
                case DeviceClass.CellPhone:
                case DeviceClass.CordlessPhone:
                case DeviceClass.SmartPhone:
                case DeviceClass.WiredPhone:
                    expected = true;
                    break;
            }

            var result = BluetoothDeviceType.Create(deviceClass);
            Assert.AreEqual(expected, result.IsValid);
        }

        [Test]
        public void Should_return_DeviceType_from_Name(
            [AllEnumValues(typeof(DeviceClass))] DeviceClass deviceClass)
        {
            var expected = deviceClass.ToString();
            var result = BluetoothDeviceType.Create(deviceClass);
            Assert.AreEqual(expected, result.Name);
        }


        [Test]
        public void Should_return_MatchingUri_from_Image(
            [AllEnumValues(typeof(DeviceClass))] DeviceClass deviceClass)
        {
            var result = BluetoothDeviceType.Create(deviceClass);

            Uri expected;
            switch (deviceClass)
            {
                //Phones
                case DeviceClass.Phone:
                case DeviceClass.CellPhone:
                case DeviceClass.CordlessPhone:
                case DeviceClass.SmartPhone:
                case DeviceClass.WiredPhone:
                    expected = new Uri("pack://application:,,,/CallWall.Shell;component/Images/Bluetooth/Phone.png");
                    break;
                //Computers
                case DeviceClass.Computer:
                case DeviceClass.DesktopComputer:
                case DeviceClass.ServerComputer:
                case DeviceClass.LaptopComputer:
                case DeviceClass.HandheldComputer:
                case DeviceClass.PdaComputer:
                case DeviceClass.WearableComputer:
                    expected = new Uri("pack://application:,,,/CallWall.Shell;component/Images/Bluetooth/Computer.png");
                    break;
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
                    expected =new Uri("pack://application:,,,/CallWall.Shell;component/Images/Bluetooth/Network.png");
                    break;
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
                    expected = new Uri("pack://application:,,,/CallWall.Shell;component/Images/Bluetooth/AudioVisual.png");
                    break;
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
                    expected = new Uri("pack://application:,,,/CallWall.Shell;component/Images/Bluetooth/Peripheral.png");
                    break;
                //Imaging/Printers/Cameras
                case DeviceClass.Imaging:
                case DeviceClass.ImagingDisplay:
                case DeviceClass.ImagingCamera:
                case DeviceClass.ImagingScanner:
                case DeviceClass.ImagingPrinter:
                    expected = new Uri("pack://application:,,,/CallWall.Shell;component/Images/Bluetooth/Imaging.png");
                    break;
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
                    expected = new Uri("pack://application:,,,/CallWall.Shell;component/Images/Bluetooth/Miscellaneous.png");
                    break;
            }

            
            Assert.AreEqual(expected, result.Image);
        }
    }
}
// ReSharper restore InconsistentNaming