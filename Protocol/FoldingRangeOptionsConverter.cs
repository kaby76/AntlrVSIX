using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Protocol
{
    //
    // Summary:
    //     Converter which serializes a boolean value to FoldingRangeProviderOptions.
    public class FoldingRangeOptionsConverter : JsonConverter
    {
        public FoldingRangeOptionsConverter() { }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JsonReader jsonReader = reader;
            if (jsonReader == null)
                throw new ArgumentNullException(nameof(reader));
            reader = jsonReader;
            if (reader.TokenType == JsonToken.Boolean)
            {
                if ((bool)JToken.ReadFrom(reader).ToObject<bool>())
                    return (object)new FoldingRangeProviderOptions();
            }
            else if (reader.TokenType == JsonToken.StartObject)
                return (object)JToken.ReadFrom(reader).ToObject<FoldingRangeProviderOptions>();
            return (object)null;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JsonWriter jsonWriter = writer;
            if (jsonWriter == null)
                throw new ArgumentNullException(nameof(writer));
            writer = jsonWriter;
            JToken.FromObject(value).WriteTo(writer, Array.Empty<JsonConverter>());
        }
    }
}
