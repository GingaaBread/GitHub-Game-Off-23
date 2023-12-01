namespace main.repository
{
    /// <summary>
    ///     This interface describes how repositories are implemented.
    ///     The base method is getting all assets from the Resource path.
    ///     To do this, the type must be set to the desired type and then the GetAll() method must be used.
    /// </summary>
    /// <typeparam name="T">The type of the asset (Card, etc.)</typeparam>
    public interface IAssetRepository<out T>
    {
        /// <summary>
        ///     Used to retrieve all values T
        /// </summary>
        /// <returns>All values as T</returns>
        T[] GetAll();
    }
}