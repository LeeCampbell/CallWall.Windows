namespace CallWall
{
    /// <summary>
    /// Factory for creating <see cref="ILogger"/> instances.
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Factory method to create an <see cref="ILogger"/> instance.
        /// </summary>
        /// <returns>Returns an instance of an <see cref="ILogger"/> for the calling type.</returns>
        ILogger CreateLogger();
    }
}