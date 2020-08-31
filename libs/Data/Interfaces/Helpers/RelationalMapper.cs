using CoEvent.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace CoEvent.Data.Helpers
{
    abstract class RelationalMapper
    {
        #region Variables
        private readonly ConcurrentDictionary<Type, Func<EntityMap>> _entities = new ConcurrentDictionary<Type, Func<EntityMap>>();
        #endregion

        #region Constructors
        public RelationalMapper(Type type)
        {
            var dbsetType = typeof(DbSet<>);
            var repositories = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => dbsetType.IsAssignableFrom(p.PropertyType)).Select(p => new EntityMap(p.PropertyType.GetGenericArguments()[0], this));
            repositories.ForEach(r => _entities.AddOrUpdate(r.EntityType, () => r, (key, current) => () => r));
        }
        #endregion

        #region Methods
        public EntityMap GetMap<T>()
        {
            return GetMap(typeof(T));
        }

        public EntityMap GetMap(Type type)
        {
            return _entities[type]();
        }

        public void AddMap(Type type, Func<EntityMap> map)
        {
            if (!_entities.ContainsKey(type)) _entities.AddOrUpdate(type, map, (key, current) => map);
        }
        #endregion
    }

    class RelationalMapper<T> : RelationalMapper
        where T : DbContext
    {
        #region Constructors
        public RelationalMapper() : base(typeof(T))
        {
        }
        #endregion
    }

    struct EntityMap
    {
        #region Properties
        public Type EntityType { get; }
        public IEnumerable<EntityProperty> Properties { get; }
        public IEnumerable<EntityKey> PrimaryKeys { get; }
        public IEnumerable<ReferenceMap> References { get; }
        public IEnumerable<ReferenceMap> Collections { get; }
        #endregion

        #region Constructors
        public EntityMap(Type type, RelationalMapper mapper)
        {
            this.EntityType = type?.GetType() ?? throw new ArgumentNullException(nameof(type));
            var properties = this.EntityType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.GetCustomAttribute<NotMappedAttribute>() == null).Select(p => new EntityProperty(p));
            this.PrimaryKeys = properties.Where(p => p.Attributes.Any(a => a is KeyAttribute)).Select(p => new EntityKey(p)).OrderBy(k => k.Order);
            this.Properties = properties.Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string)); // TODO: Need to handle all types.
            var values = this.Properties;
            this.References = properties.Where(p => p.PropertyType.IsClass).Select(p => new ReferenceMap(p, values, mapper));
            this.Collections = properties.Where(p => typeof(IEnumerable<>).IsAssignableFrom(p.PropertyType)).Select(p => new ReferenceMap(p, values, mapper));
        }
        #endregion

        #region Methods
        public IEnumerable<object> GetPrimaryKeyValues(object entity)
        {
            return this.PrimaryKeys.Select(p => p.Property.GetValue(entity));
        }
        #endregion
    }

    struct ReferenceMap
    {
        #region Variable
        private readonly RelationalMapper _mapper;
        #endregion

        #region Properties
        public EntityProperty Property { get; }
        public IEnumerable<EntityProperty> ForeignKeys { get; }
        #endregion

        #region Constructors
        public ReferenceMap(EntityProperty property, IEnumerable<EntityProperty> properties, RelationalMapper mapper)
        {
            this.Property = property;
            _mapper = mapper;
            mapper.AddMap(this.Property.PropertyType, () => new EntityMap(property.PropertyType, mapper));

            if (!typeof(IEnumerable<>).IsAssignableFrom(property.PropertyType))
            {
                var fka = (ForeignKeyAttribute)property.Attributes.SingleOrDefault(a => a is ForeignKeyAttribute);
                if (fka != null)
                {
                    // Check if it's a multi key.
                    var keys = fka.Name.Split(',').Select(n => n.Trim());
                    this.ForeignKeys = properties.Where(p => keys.Contains(p.Name));
                }
                else
                {
                    // Get the foreign keys that are referencing this property.
                    this.ForeignKeys = properties.Where(p => p.Attributes.Any(a => a is ForeignKeyAttribute && ((ForeignKeyAttribute)a).Name.Equals(property.Name))).OrderBy(p => (p.Attributes.SingleOrDefault(a => a is ColumnAttribute) as ColumnAttribute)?.Order ?? 0);
                }
            }
            else
            {
                this.ForeignKeys = properties.Where(p => p.Attributes.Any(a => a is KeyAttribute)).OrderBy(p => (p.Attributes.SingleOrDefault(a => a is ColumnAttribute) as ColumnAttribute)?.Order ?? 0);
            }
        }
        #endregion

        #region Methods
        public EntityMap GetReference()
        {
            return _mapper.GetMap(this.Property.PropertyType);
        }
        #endregion
    }

    struct EntityProperty
    {
        #region Properties
        public string Name { get; }
        public PropertyInfo Property { get; }
        public Type PropertyType { get { return this.Property.PropertyType; } }

        public IEnumerable<Attribute> Attributes { get; }
        #endregion

        #region Constructors
        public EntityProperty(PropertyInfo property)
        {
            this.Name = property.Name;
            this.Property = property;
            this.Attributes = property.GetCustomAttributes();
        }
        #endregion

        #region Properties
        public void SetValue(object entity, object value)
        {
            this.Property.SetValue(entity, value);
        }

        public object GetValue(object entity)
        {
            return this.Property.GetValue(entity);
        }
        #endregion
    }

    struct EntityKey
    {
        #region Properties
        public EntityProperty Property { get; }
        public int Order { get; }
        #endregion

        #region Constructors
        public EntityKey(EntityProperty property)
        {
            this.Property = property;
            var attr = (ColumnAttribute)property.Attributes.Single(a => a is ColumnAttribute);
            this.Order = attr?.Order ?? 0;
        }
        #endregion
    }
}
