namespace CallWall.Windows
{
    public abstract class AuthorizationStatus
    {
        public static readonly AuthorizationStatus NotAuthorized = new NotAuthorizedStatus();
        public static readonly AuthorizationStatus Authorized = new AuthorizedStatus();
        public static readonly AuthorizationStatus Processing = new ProcessingStatus();


        private AuthorizationStatus()
        {
        }

        public virtual bool IsAuthorized
        {
            get { return false; }
        }

        public virtual bool IsProcessing
        {
            get { return false; }
        }

        private sealed class NotAuthorizedStatus : AuthorizationStatus
        {
        }
        private sealed class AuthorizedStatus : AuthorizationStatus
        {
            public override bool IsAuthorized { get { return true; } }
        }

        private sealed class ProcessingStatus : AuthorizationStatus
        {
            public override bool IsProcessing { get { return true; } }
        }
    }
}