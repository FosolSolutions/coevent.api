using System.Collections.Generic;

namespace CoEvent.Data.Interfaces
{
    /// <summary>
    /// IUpdatableService interface, provides generic functions to update the datasource.
    /// </summary>
    /// <typeparam name="ModelT"></typeparam>
    public interface IUpdatableService<ModelT> : IReadableService<ModelT>
        where ModelT : class
    {
        #region Methods
        /// <summary>
        /// Add the model to the in-memory collection so that it can be saved to the datasource.
        /// </summary>
        /// <param name="model"></param>
        void Add(ModelT model);

        /// <summary>
        /// Add the models to the in-memory collection so that they can be saved to the datasource.
        /// </summary>
        /// <param name="models"></param>
        void AddRange(IEnumerable<ModelT> models);

        /// <summary>
        /// Add the models to the in-memory collection so that they can be saved to the datasource.
        /// </summary>
        /// <param name="models"></param>
        void AddRange(params ModelT[] models);

        /// <summary>
        /// Remove the model from the in-memory collection so that it can be deleted from the datasource.
        /// </summary>
        /// <param name="model"></param>
        void Remove(ModelT model);

        /// <summary>
        /// Remove the models from the in-memory collection so that they can be deleted from the datasource.
        /// </summary>
        /// <param name="models"></param>
        void RemoveRange(IEnumerable<ModelT> models);

        /// <summary>
        /// Remove the models from the in-memory collection so that they can be deleted from the datasource.
        /// </summary>
        /// <param name="models"></param>
        void RemoveRange(params ModelT[] models);

        /// <summary>
        /// Update the model in the in-memory collection so that it can be saved to the datasource.
        /// </summary>
        /// <param name="model"></param>
        void Update(ModelT model);

        /// <summary>
        /// Update the models in the in-memory collection so that they can be saved to the datasource.
        /// </summary>
        /// <param name="models"></param>
        void UpdateRange(IEnumerable<ModelT> models);

        /// <summary>
        /// Update the models in the in-memory collection so that they can be saved to the datasource.
        /// </summary>
        /// <param name="models"></param>
        void UpdateRange(params ModelT[] models);
        #endregion
    }
}
