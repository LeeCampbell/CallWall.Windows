namespace CallWall.Windows.Google.Providers.Gmail.Imap
{
    internal sealed class SelectFolderOperation : ImapOperationBase
    {
        private readonly string _command;

        public SelectFolderOperation(string folder, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _command = string.Format("SELECT \"{0}\"", folder);
        }

        protected override string Command
        {
            get { return _command; }
        }
    }
}