namespace CoEvent.Data.Interfaces
{
    /// <summary>
    /// IReadableService interface, provides generic
    /// </summary>
    /// <typeparam name="ModelT"></typeparam>
    public interface IReadableService<ModelT>
        where ModelT : class
    {
        /// <summary>
        /// Get the model for the specified key.
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        ModelT Find(params object[] keyValues);
    }
}
