using System;
using System.IO;
using Newtonsoft.Json;

namespace CSharpWars.Common.Extensions
{
    public static class SerializationExtensions
    {
        public static String Serialize<T>(this T objectToSerialize)
        {
            if (objectToSerialize == null) return String.Empty;
            using (var writer = new StringWriter())
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, objectToSerialize);
                return writer.ToString();
            }
        }

        public static T Deserialize<T>(this String objectToDeserialize)
        {
            if (objectToDeserialize == null) return default(T);
            using (var stringReader = new StringReader(objectToDeserialize))
            {
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    var serializer = new JsonSerializer();
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}