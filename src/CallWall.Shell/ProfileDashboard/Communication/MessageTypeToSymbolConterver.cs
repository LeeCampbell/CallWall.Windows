using System;
using System.Globalization;
using System.Windows.Data;
using CallWall.Contract.Communication;

namespace CallWall.ProfileDashboard.Communication
{
    public sealed class MessageTypeToSymbolConterver : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (MessageType)value;
            switch (source)
            {
                case MessageType.Unknown:
                    break;
                case MessageType.Email:
                    return char.ConvertFromUtf32(0xE119);   //135
                case MessageType.InstantMessage:
                case MessageType.Tweet:
                    return char.ConvertFromUtf32(0xE134);
                case MessageType.Sms:
                    return char.ConvertFromUtf32(0xE15F);   //return "SMS";
                //case PhoneCall 0xE13A
                //Contact card 0xE181
                //Map 0xE1C4
                //Close X 0xE21C
                //Incoming xE093    E253  2B0A   2937
                //OUtgoing          E143  2B08   2934
            }
            //throw new ArgumentOutOfRangeException();
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
