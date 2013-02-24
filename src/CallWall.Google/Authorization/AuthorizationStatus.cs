namespace CallWall.Google.Authorization
{
    public abstract class AuthorizationStatus
    {
        public static readonly AuthorizationStatus Uninitialized = new UninitializedStatus();
        public static readonly AuthorizationStatus NotAuthorized = new NotAuthorizedStatus();
        public static readonly AuthorizationStatus Authorized = new AuthorizedStatus();
        public static readonly AuthorizationStatus Processing = new ProcessingStatus();
        public static AuthorizationStatus Error(string errorMessage)
        {
            return new ErrorStatus(errorMessage);
        }

        private AuthorizationStatus()
        { }

        public virtual bool IsInitialized { get { return false; } }
        public virtual bool IsAuthorized { get { return false; } }
        public virtual bool IsProcessing { get { return false; } }
        public virtual bool HasError { get { return false; } }
        public virtual string ErrorMessage { get { return null; } }

        private sealed class UninitializedStatus : AuthorizationStatus
        {
            public override bool HasError { get { return true; } }
            public override string ErrorMessage { get { return "No call-back has been registered via the RegisterAuthorizationCallback method"; } }
        }

        private sealed class NotAuthorizedStatus : AuthorizationStatus
        {
            public override bool IsInitialized { get { return true; } }
        }
        private sealed class AuthorizedStatus : AuthorizationStatus
        {
            public override bool IsInitialized { get { return true; } }
            public override bool IsAuthorized { get { return true; } }
        }

        private sealed class ProcessingStatus : AuthorizationStatus
        {
            public override bool IsInitialized { get { return true; } }
            public override bool IsProcessing { get { return true; } }
        }

        private sealed class ErrorStatus : AuthorizationStatus
        {
            private readonly string _errorMessage;

            public ErrorStatus(string errorMessage)
            {
                _errorMessage = errorMessage;
            }
            public override bool IsInitialized { get { return true; } }
            public override bool HasError { get { return true; } }
            public override string ErrorMessage { get { return _errorMessage; } }
        }

    }
}
