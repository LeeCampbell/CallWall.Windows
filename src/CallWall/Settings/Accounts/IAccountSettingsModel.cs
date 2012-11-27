namespace CallWall.Settings.Accounts
{
    public interface IAccountSettingsModel
    {
        bool RequiresSetup { get; }
    }

    public sealed class AccountSettingsModel : IAccountSettingsModel
    {
        #region Implementation of IAccountSettingsModel

        public bool RequiresSetup
        {
            get { return true; }
        }

        #endregion
    }
}