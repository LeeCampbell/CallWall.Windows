namespace CallWall.Windows.Shell
{
    public abstract class ViewModelStatus
    {
        public static readonly ViewModelStatus Idle = new IdleStatus();
        public static readonly ViewModelStatus Processing = new ProcessingStatus();
        public static ViewModelStatus Error(string errorMessage)
        {
            return new ErrorStatus(errorMessage);
        }

        private ViewModelStatus()
        { }
        public virtual bool IsProcessing { get { return false; } }
        public virtual bool HasError { get { return false; } }
        public virtual string ErrorMessage { get { return null; } }

        private sealed class IdleStatus : ViewModelStatus
        {
        }

        private sealed class ProcessingStatus : ViewModelStatus
        {
            public override bool IsProcessing { get { return true; } }
        }

        private sealed class ErrorStatus : ViewModelStatus
        {
            private readonly string _errorMessage;

            public ErrorStatus(string errorMessage)
            {
                _errorMessage = errorMessage;
            }

            public override bool HasError { get { return true; } }
            public override string ErrorMessage { get { return _errorMessage; } }
        }
    }
}