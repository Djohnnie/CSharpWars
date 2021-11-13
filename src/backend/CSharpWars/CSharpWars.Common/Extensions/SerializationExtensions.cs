using System.Text.Json;

namespace CSharpWars.Common.Extensions;

public static class SerializationExtensions
{
    public static string Serialize<T>(this T objectToSerialize)
    {
        if (objectToSerialize == null) return string.Empty;

        return JsonSerializer.Serialize(objectToSerialize);
    }

    public static T Deserialize<T>(this string objectToDeserialize)
    {
        if (string.IsNullOrEmpty(objectToDeserialize)) return default(T);

        return JsonSerializer.Deserialize<T>(objectToDeserialize);
    }
}