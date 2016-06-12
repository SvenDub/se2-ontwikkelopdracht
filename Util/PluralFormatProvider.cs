using System;

namespace Util
{
    /// <summary>
    ///     Formatter that handles plurals.
    /// </summary>
    public class PluralFormatProvider : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        ///     Get this format provider.
        /// </summary>
        public object GetFormat(Type formatType)
        {
            return this;
        }

        /// <summary>
        ///     Format a string by parsing plurals.
        /// </summary>
        /// <example>
        ///     "I have {0:car;cars}."
        /// </example>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            string[] forms = format.Split(';');
            int value = (int) arg;
            int form = value == 1 ? 0 : 1;
            return value.ToString() + " " + forms[form];
        }
    }
}