using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Protocol
{

    /// <summary>Converter to translate to and from SumTypes.</summary>
    public class SumConverter : JsonConverter
    {
        private readonly object converterSyncObj;
        private JsonConverter primitiveConverter;

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <inheritdoc />
        public override object ReadJson(
          JsonReader reader,
          Type objectType,
          object existingValue,
          JsonSerializer serializer)
        {
            Dictionary<Type, ConstructorInfo> dictionary
                //= objectType
                //.GetTypeInfo()
                //.DeclaredConstructors
                ;
            JToken jtoken = JToken.ReadFrom(reader);
            object converterSyncObj = this.converterSyncObj;
            bool lockTaken = false;
            try
            {
                //Monitor.Enter(converterSyncObj, ref lockTaken);
                // ISSUE: method pointer
                Tuple<int, Type> tuple = null;
                //foreach (Tuple<int, Type> tuple in ((IEnumerable<Type>)objectType.GenericTypeArguments).Select(
                //    SumConverter.9__3_2 ?? (SumConverter.9__3_2 = new Func<Type, int, Tuple<int, Type>>()))
                {
                    try
                    {
                        int num = tuple.Item1;
                        Type key = tuple.Item2;
                        ((ICollection<JsonConverter>)serializer.Converters).Add(this.primitiveConverter);
                        object[] parameters = new object[1]
                        {
                            jtoken.ToObject(key, serializer)
                        };
                        //return dictionary[key].Invoke(parameters);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        ((ICollection<JsonConverter>)serializer.Converters).Remove(this.primitiveConverter);
                    }
                }
            }
            finally
            {
                //if (lockTaken)
                //    Monitor.Exit(converterSyncObj);
            }
            throw new JsonSerializationException();
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JsonWriter jsonWriter = writer;
            if (jsonWriter == null)
                throw new ArgumentNullException(nameof(writer));
            writer = jsonWriter;
            object obj = ((ISumType)value).Value;
            if (obj == null)
                return;
            JToken.FromObject(obj).WriteTo(writer, Array.Empty<JsonConverter>());
        }

        public SumConverter()
            : base()
        {
            this.converterSyncObj = new object();
            this.primitiveConverter = (JsonConverter)new StrictPrimitiveConverter();
        }
    }
}
