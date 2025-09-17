using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace weatherapp.utils
{
    public class JsonUtil
    {
        public static JsonElement parsejson(string json, string property)
        {
            using var doc = JsonDocument.Parse(json);
            var element = doc.RootElement;
            foreach (var part in property.Split('.'))
            {
                if (int.TryParse(part, out int index))
                {
                    if (element.ValueKind == JsonValueKind.Array && index < element.GetArrayLength())
                    {
                        element = element[index];
                    }
                    else
                    {
                        throw new IndexOutOfRangeException($"Array index '{index}' is out of range or element is not an array.");
                    }
                }
                else
                {
                    if (element.TryGetProperty(part, out var next))
                    {
                        element = next;
                    }
                    else
                    {
                        throw new KeyNotFoundException($"Property '{part}' not found in JSON at path '{property}'.");
                    }
                }
            }
            return element;
        }

        public static T parsejson<T>(string json, string property)
        {
            using var doc = JsonDocument.Parse(json);
            var element = doc.RootElement;
            foreach (var part in property.Split('.'))
            {
                if (int.TryParse(part, out int index))
                {
                    if (element.ValueKind == JsonValueKind.Array && index < element.GetArrayLength())
                    {
                        element = element[index];
                    }
                    else
                    {
                        throw new IndexOutOfRangeException($"Array index '{index}' is out of range or element is not an array.");
                    }
                }
                else
                {
                    if (element.TryGetProperty(part, out var next))
                    {
                        element = next;
                    }
                    else
                    {
                        throw new KeyNotFoundException($"Property '{part}' not found in JSON at path '{property}'.");
                    }
                }
            }

            var type = typeof(T);
            if (type == typeof(string))
            {
                return (T)(object)element.GetString();
            }
            else if (type == typeof(double))
            {
                return (T)(object)element.GetDouble();
            }
            else if (type == typeof(int))
            {
                return (T)(object)element.GetInt32();
            }
            else if (type == typeof(bool))
            {
                return (T)(object)element.GetBoolean();
            }
            else
            {
                throw new NotSupportedException($"Type {type.Name} is not supported");
            }
        }
    }
}
