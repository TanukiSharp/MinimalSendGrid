using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendGridCore
{
    /// <summary>
    /// Json generator that can stringify a JSON object, based on IDictionary(string, object) type.
    /// </summary>
    public static class JsonGenerator
    {
        private const int Indentation = 4;

        /// <summary>
        /// Stringify any object to a JSON string representation.
        /// </summary>
        /// <param name="json">The JSON object to stringify.</param>
        /// <remarks>The output JSON is not minified by default.</remarks>
        /// <returns>Returns the string representation of the given JSON object.</returns>
        public static string Stringify(object json)
        {
            return Stringify(json, false);
        }

        /// <summary>
        /// Stringify any object to a JSON string representation.
        /// </summary>
        /// <param name="json">The JSON object to stringify.</param>
        /// <param name="isMinified">Tells whether to output minified JSON data.</param>
        /// <returns>Returns the string representation of the given JSON object.</returns>
        public static string Stringify(object json, bool isMinified)
        {
            var sb = new StringBuilder();
            Stringify(sb, "<root>", json, isMinified ? -1 : 0);
            if (isMinified == false)
                sb.AppendLine();
            return sb.ToString();
        }

        private static void Stringify(StringBuilder sb, string memberName, object value, int level)
        {
            if (value == null)
                sb.Append("null");

            else if (value is string)
                sb.AppendFormat("\"{0}\"", (string)value);

            else if (value is float)
                sb.Append(StringifyNumber((float)value));

            else if (value is bool)
                sb.Append((bool)value ? "true" : "false");

            else if (value is Array)
                StringifyArray(sb, memberName, (Array)value, level);

            else if (value is IDictionary<string, object>)
                StringifyObject(sb, memberName, (IDictionary<string, object>)value, level);

            else if (value is double)
                sb.Append(StringifyNumber((float)((double)value)));
            else if (value is int)
                sb.Append(StringifyNumber(((int)value)));
            else if (value is uint)
                sb.Append(StringifyNumber(((uint)value)));
            else if (value is short)
                sb.Append(StringifyNumber(((short)value)));
            else if (value is ushort)
                sb.Append(StringifyNumber(((ushort)value)));
            else if (value is long)
                sb.Append(StringifyNumber(((long)value)));
            else if (value is ulong)
                sb.Append(StringifyNumber(((ulong)value)));
            else if (value is byte)
                sb.Append(StringifyNumber(((byte)value)));
            else if (value is sbyte)
                sb.Append(StringifyNumber(((sbyte)value)));
            else
                throw new FormatException($"Invalid type ({value.GetType()}) for member '{memberName}'");
        }

        private static string StringifyNumber(float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        private static void StringifyObject(StringBuilder sb, string memberName, IDictionary<string, object> value, int level)
        {
            sb.Append('{');

            if (level >= 0)
                sb.AppendLine();

            var n = 0;
            var nMax = value.Keys.Count;

            foreach (var kv in value)
            {
                if (level >= 0)
                    sb.Append(new string(' ', (level + 1) * Indentation));

                sb.AppendFormat("\"{0}\":", kv.Key);

                if (level >= 0)
                    sb.Append(' ');

                Stringify(sb, memberName + "." + kv.Key, kv.Value, level >= 0 ? level + 1 : level);

                if (n < nMax - 1)
                {
                    sb.Append(',');
                    if (level >= 0)
                        sb.AppendLine();
                }

                n++;
            }

            if (level >= 0)
                sb.AppendLine();

            if (level > 0)
                sb.Append(new string(' ', level * Indentation));

            sb.Append('}');
        }

        private static void StringifyArray(StringBuilder sb, string memberName, Array value, int level)
        {
            sb.Append('[');

            if (level >= 0)
                sb.AppendLine();

            int i;
            for (i = 0; i < value.Length; i++)
            {
                if (level >= 0)
                    sb.Append(new string(' ', (level + 1) * Indentation));

                Stringify(sb, $"{memberName}[{i}]", value.GetValue(i), level >= 0 ? level + 1 : level);

                if (i < value.Length - 1)
                {
                    sb.Append(',');
                    if (level >= 0)
                        sb.AppendLine();
                }
            }

            if (level >= 0)
                sb.AppendLine();

            if (level > 0)
                sb.Append(new string(' ', level * Indentation));

            sb.Append(']');
        }
    }
}
