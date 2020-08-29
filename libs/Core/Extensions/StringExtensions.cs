using System;

namespace CoEvent.Core.Extensions
{
    /// <summary>
    /// StringExtensions static class, provides extension methods for string objects.
    /// </summary>
    public static class StringExtensions
    {
        #region Methods
        /// <summary>
        /// Converts the string value to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <exception cref="InvalidCastException">If the value is not castable to the specified <typeparamref name="T"/>.</exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string value)
        {
            return (T)value.ConvertTo(typeof(T));
        }

        /// <summary>
        /// Converts the string value to the specified 'type'.
        /// </summary>
        /// <exception cref="InvalidCastException">If the value is not castable to the specified 'type'.</exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ConvertTo(this string value, Type type)
        {
            if (value == null && Nullable.GetUnderlyingType(type) == null)
                throw new InvalidCastException($"Unable to cast null value to type '{type.Name}'.");

            // TODO: serialization.
            // TODO: enums.
            return Convert.ChangeType(value, type);
        }
        #endregion
    }
}
