using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace Protocol
{
    //
    // Summary:
    //     Converter which offers custom serialization for Microsoft.VisualStudio.LanguageServer.Protocol.TextDocumentSyncKind
    //     enum to a Microsoft.VisualStudio.LanguageServer.Protocol.TextDocumentSyncOptions
    //     object.
    //
    // Remarks:
    //     This is to support backwards compatibility for the protocol.
    public class TextDocumentSyncConverter : JsonConverter
    {
        public TextDocumentSyncConverter() { }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        //
        // Summary:
        //     Deserializes a json value to a Microsoft.VisualStudio.LanguageServer.Protocol.TextDocumentSyncOptions
        //     object.
        //
        // Parameters:
        //   reader:
        //     Reader from which to read json value.
        //
        //   objectType:
        //     Type of the json value.
        //
        //   existingValue:
        //     Existing value.
        //
        //   serializer:
        //     Default serializer.
        //
        // Returns:
        //     A Microsoft.VisualStudio.LanguageServer.Protocol.TextDocumentSyncOptions which
        //     matches the json value.
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JsonReader jsonReader = reader;
            if (jsonReader == null)
                throw new ArgumentNullException(nameof(reader));
            reader = jsonReader;
            if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
            {
                TextDocumentSyncOptions documentSyncOptions = new TextDocumentSyncOptions();
                documentSyncOptions.OpenClose = true;
                documentSyncOptions.Change = (TextDocumentSyncKind)int.Parse(reader.Value.ToString(), NumberStyles.Integer, (IFormatProvider)CultureInfo.CurrentCulture);
                SaveOptions saveOptions = new SaveOptions();
                saveOptions.IncludeText = false;
                documentSyncOptions.Save = saveOptions;
                return (object)documentSyncOptions;
            }
            if (reader.TokenType == JsonToken.String)
                return (object)JsonConvert.DeserializeObject<TextDocumentSyncOptions>(reader.Value.ToString());
            if (reader.TokenType == JsonToken.StartObject)
                return (object)JToken.ReadFrom(reader).ToObject<TextDocumentSyncOptions>();
            if (reader.TokenType != JsonToken.Null)
                throw new JsonSerializationException(reader.Value.ToString());
            TextDocumentSyncOptions documentSyncOptions1 = new TextDocumentSyncOptions();
            documentSyncOptions1.OpenClose = true;
            documentSyncOptions1.Change = TextDocumentSyncKind.None;
            SaveOptions saveOptions1 = new SaveOptions();
            saveOptions1.IncludeText = false;
            documentSyncOptions1.Save = saveOptions1;
            return (object)documentSyncOptions1;
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
