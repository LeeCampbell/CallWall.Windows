using System.Text;
using System.Threading.Tasks;

namespace CallWall.Settings
{
    public sealed class SettingsModule
    {
        public SettingsModule()
        {

        }

        public void ShowSettings()
        {

        }
    }


    //SettingsModel
    //  IE<IConnectionSetting> ConnectionSettings
    //  IE<IProvderSetting> ProviderSettings

    //SettingsViewModel
    //  IE<SubView> ConnectionSettings
    //  IE<SubView> ProviderSettings

    //SubView
    //  string Name
    //  Uri Image
    //  DelgateCommand Open

    //IConnectionSetting
    //  string Name
    //  Uri Image
    //  ..other things to actual manage the thing

    //IProvderSetting
    //  string Name
    //  Uri Image
    //  ..other things to actual manage the thing
}
