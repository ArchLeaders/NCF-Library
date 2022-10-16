namespace BntxLibrary.Core
{
    /// <summary>
    /// Represents the common interface for <see cref="BntxFile"/> data instances.
    /// </summary>
    public interface IResData
    {
        // ---- METHODS ------------------------------------------------------------------------------------------------

        /// <summary>
        /// Loads raw data from the <paramref name="loader"/> data stream into instances.
        /// </summary>
        /// <param name="loader">The <see cref="BntxFileLoader"/> to load data with.</param>
        void Load(BntxFileLoader loader);
        
        /// <summary>
        /// Saves header data of the instance and queues referenced data in the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="BntxFileWriter"/> to save headers and queue data with.</param>
        void Save(BntxFileWriter writer);
    }
}
