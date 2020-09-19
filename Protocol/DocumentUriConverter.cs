using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Protocol
{
    //
    // Summary:
    //     TODO: document
    public class DocumentUriConverter : JsonConverter
    {
        public DocumentUriConverter() { }

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
            if (reader.TokenType == JsonToken.String)
                return (object)new Uri((string)JToken.ReadFrom(reader).ToObject<string>());
            if (reader.TokenType == JsonToken.Null)
                return (object)null;
            throw new JsonSerializationException(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JsonWriter jsonWriter = writer;
            if (jsonWriter == null)
                throw new ArgumentNullException(nameof(writer));
            writer = jsonWriter;
            Uri uri = value as Uri;
            if ((object)uri == null)
                throw new ArgumentException("value must be of type Uri");
            JToken.FromObject((object)uri.AbsoluteUri).WriteTo(writer, Array.Empty<JsonConverter>());
        }
    }
}
