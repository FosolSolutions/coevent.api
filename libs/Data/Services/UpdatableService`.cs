using CoEvent.Core.Extensions;
using CoEvent.Data.Interfaces;
using CoEvent.Data.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CoEvent.Data.Services
{
    /// <summary>
    /// UpdatableService abstract class, provides a common generic implementation for all services.  This provides a way to manage entities in the datasource.
    /// </summary>
    /// <typeparam name="EntityT"></typeparam>
    /// <typeparam name="ModelT"></typeparam>
    public abstract class UpdatableService<EntityT, ModelT> : ReadableService<EntityT, ModelT>, IUpdatableService<ModelT>, IDisposable
        where EntityT : class
        where ModelT : class
    {
        #region Variables
        private readonly ConcurrentDictionary<EntityT, ModelT> _tracking = new ConcurrentDictionary<EntityT, ModelT>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UpdatableService object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="source"></param>
        internal UpdatableService(IDataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sync models with the tracked entities.
        /// Copies property values from the entity to the model.
        /// </summary>
        protected void Sync()
        {
            foreach (var track in _tracking)
            {
                this.Map(track.Key, track.Value);
            }
            _tracking.Clear();
        }

        /// <summary>
        /// Keep a reference to the entity and it's model so that they can be updated after a commit to the datasource.
        /// This is used for generated property values.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        protected void Track(EntityT entity, ModelT model)
        {
            _tracking.AddOrUpdate(entity, model, (key, existingValue) =>
            {
                return model;
            });
        }

        /// <summary>
        /// Add the specified entity to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="entity"></param>
        protected void Add(EntityT entity)
        {
            this.VerifyPrincipal(true);
            this.Context.Set<EntityT>().Add(entity);
        }

        /// <summary>
        /// Add the specified model to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="model"></param>
        public virtual void Add(ModelT model)
        {
            var entity = this.Map(model);
            this.Add(entity);
            Track(entity, model);
        }

        /// <summary>
        /// Add the specified models to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="models"></param>
        public virtual void AddRange(IEnumerable<ModelT> models)
        {
            this.VerifyPrincipal(true);
            var entities = models.Select(m => new Tuple<EntityT, ModelT>(this.Map(m), m));
            entities.ForEach(e => Track(e.Item1, e.Item2));
            this.Context.Set<EntityT>().AddRange(entities.Select(t => t.Item1));
        }

        /// <summary>
        /// Add the specified models to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="models"></param>
        public virtual void AddRange(params ModelT[] models)
        {
            this.AddRange(models.AsEnumerable());
        }

        /// <summary>
        /// Remove the entity from the in-memory collection, so that it can be removed from the datasource on commit.
        /// </summary>
        /// <param name="entity"></param>
        protected void Remove(EntityT entity)
        {
            this.VerifyPrincipal(true);
            this.Context.Set<EntityT>().Remove(entity);
        }

        /// <summary>
        /// Remove the specified model from the in-memory collection, so that it can be removed to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="model"></param>
        public virtual void Remove(ModelT model)
        {
            var entity = this.Source.Mapper.Map(model, this.Find<EntityT>(model));
            Remove(entity);
        }

        /// <summary>
        /// Remove the specified models from the in-memory collection, so that it can be removed to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public virtual void RemoveRange(IEnumerable<ModelT> models)
        {
            this.VerifyPrincipal(true);
            // TODO: Need to rewrite because this will make a separate request for each model.
            var entities = models.Select(m => this.Find<EntityT>(m));
            this.Context.Set<EntityT>().RemoveRange(entities);
        }

        /// <summary>
        /// Remove the specified models from the in-memory collection, so that it can be removed to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public virtual void RemoveRange(params ModelT[] models)
        {
            this.RemoveRange(models.AsEnumerable());
        }

        /// <summary>
        /// Update the specified entity from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="entity"></param>
        protected void Update(EntityT entity)
        {
            this.VerifyPrincipal(true);
            this.Context.Set<EntityT>().Update(entity);

            var baseEntity = entity as Entities.BaseEntity;
            if (baseEntity != null)
            {
                // This is required because EF is broken.
                this.Context.Entry(entity).Property(nameof(Entities.BaseEntity.RowVersion)).OriginalValue = (entity as Entities.BaseEntity).RowVersion;

                // Updates are not allowed to change who added and when it was added.
                var addedById = this.Context.Entry(entity).Property(nameof(Entities.BaseEntity.AddedById));
                addedById.CurrentValue = addedById.OriginalValue;
                var addedOn = this.Context.Entry(entity).Property(nameof(Entities.BaseEntity.AddedOn));
                addedOn.CurrentValue = addedOn.OriginalValue;
            }
        }

        /// <summary>
        /// Update the specified model from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="model"></param>
        public virtual void Update(ModelT model)
        {
            var entity = this.Source.Mapper.Map(model, this.Find<EntityT>(model));
            this.Update(entity);
            Track(entity, model);
        }

        /// <summary>
        /// Update the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public virtual void UpdateRange(IEnumerable<ModelT> models)
        {
            this.VerifyPrincipal(true);
            // TODO: Need to rewrite because this will make a separate request for each model.
            var entities = models.Select(m => new Tuple<EntityT, ModelT>(this.Map(m), m));
            entities.ForEach(t => Track(t.Item1, t.Item2));
            this.Context.Set<EntityT>().UpdateRange(entities.Select(t => t.Item1));
        }

        /// <summary>
        /// Update the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public virtual void UpdateRange(params ModelT[] models)
        {
            this.UpdateRange(models.AsEnumerable());
        }

        /// <summary>
        /// Clear tracking references.
        /// </summary>
        public void Dispose()
        {
            _tracking.Clear();
        }
        #endregion
    }
}
