using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Reflection;

namespace CoEvent.Data
{
    /// <summary>
    /// ModelBuilderExtensions static class, provides extension methods for ModelBuilder.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        #region Methods
        public static ModelBuilder UseValueConverterForType<T>(this ModelBuilder modelBuilder, ValueConverter converter)
        {
            return modelBuilder.UseValueConverterForType(typeof(T), converter);
        }

        public static ModelBuilder UseValueConverterForType(this ModelBuilder modelBuilder, Type type, ValueConverter converter)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == type);
                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name)
                        .HasConversion(converter);
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// Applies all of the IEntityTypeConfiguration objects in all of the assemblies of the current domain.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyAllConfigurations(this ModelBuilder modelBuilder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                modelBuilder.ApplyAllConfigurations(assembly);
            }

            return modelBuilder;
        }

        /// <summary>
        /// Applies all of the IEntityTypeConfiguration objects in the assembly of the specified type.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="type"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyAllConfigurations(this ModelBuilder modelBuilder, Type type, CoEventContext context = null)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return modelBuilder.ApplyAllConfigurations(type.Assembly, context);
        }

        /// <summary>
        /// Applies all of the IEntityTypeConfiguration objects in the specified assembly.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="assembly"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyAllConfigurations(this ModelBuilder modelBuilder, Assembly assembly, CoEventContext context = null)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            // Find all the configuration classes.
            var type = typeof(IEntityTypeConfiguration<>);
            var configurations = assembly.GetTypes().Where(t => t.IsClass && t.GetInterfaces().Any(i => i.Name.Equals(type.Name)));

            // Fetch the ApplyConfiguration method so that it can be called on each configuration.
            var method = typeof(ModelBuilder).GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name.Equals(nameof(ModelBuilder.ApplyConfiguration)) && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == type).First();
            foreach (var config in configurations)
            {
                if (!config.ContainsGenericParameters)
                {
                    var includeContext = config.GetConstructors().Any(c => c.GetParameters().Any(p => p.ParameterType == typeof(CoEventContext)));
                    var entityConfig = includeContext ? Activator.CreateInstance(config, context) : Activator.CreateInstance(config);
                    var entityType = config.GetInterfaces().FirstOrDefault().GetGenericArguments()[0];
                    var applyConfigurationMethod = method.MakeGenericMethod(entityType);
                    applyConfigurationMethod.Invoke(modelBuilder, new[] { entityConfig });
                }
            }

            return modelBuilder;
        }
        #endregion
    }
}
