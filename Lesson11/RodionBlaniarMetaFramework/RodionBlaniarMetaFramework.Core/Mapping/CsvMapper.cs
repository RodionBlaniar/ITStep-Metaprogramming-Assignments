using RodionBlaniarMetaFramework.Core.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RodionBlaniarMetaFramework.Core.Mapping
{
    public class CsvMapper<T> where T : new()
    {
        public List<T> Map(string filePath)
        {
            List<T> result = new List<T>();
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length == 0)
                return result;

            string[] headers = lines[0].Split(',');
            Dictionary<string, int> columnIndex = new Dictionary<string, int>();

            for (int i = 0; i < headers.Length; i++)
            {
                columnIndex[headers[i].Trim().ToLower()] = i;
            }

            PropertyInfo[] properties = typeof(T).GetProperties();

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                string[] values = lines[i].Split(',');
                T obj = new T();

                foreach (PropertyInfo prop in properties)
                {
                    if (Attribute.IsDefined(prop, typeof(IgnoreAttribute)))
                        continue;

                    ColumnAttribute colAttr = prop.GetCustomAttribute<ColumnAttribute>();
                    if (colAttr == null)
                        continue;

                    string colName = colAttr.Name.ToLower();
                    if (!columnIndex.ContainsKey(colName))
                        continue;

                    int idx = columnIndex[colName];
                    if (idx >= values.Length)
                        continue;

                    string value = values[idx].Trim();

                    try
                    {
                        object converted = ConvertValue(value, prop.PropertyType);
                        prop.SetValue(obj, converted);
                    }
                    catch
                    {
                    }
                }

                result.Add(obj);
            }

            return result;
        }

        private object ConvertValue(string value, Type targetType)
        {
            if (targetType == typeof(string))
                return value;

            if (targetType == typeof(int))
                return int.Parse(value);

            if (targetType == typeof(double))
                return double.Parse(value);

            if (targetType == typeof(decimal))
                return decimal.Parse(value);

            if (targetType == typeof(bool))
                return bool.Parse(value);

            return value;
        }
    }
}