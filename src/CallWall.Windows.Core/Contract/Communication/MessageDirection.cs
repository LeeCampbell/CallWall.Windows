namespace CallWall.Windows.Contract.Communication
{
    public enum MessageDirection
    {
        /// <summary>
        /// The message was received, i.e. Sent from someone else to you.
        /// </summary>
        Inbound,

        /// <summary>
        /// The message was sent, i.e. Sent from you to someone else.
        /// </summary>
        Outbound
    }
}