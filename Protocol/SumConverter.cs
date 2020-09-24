using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Protocol
{

    public class SumConverter : JsonConverter
    {
        private readonly object converterSyncObj;
        private JsonConverter primitiveConverter;

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(
          JsonReader reader,
          Type objectType,
          object existingValue,
          JsonSerializer serializer)
        {
            ConstructorInfo[] a = objectType.GetConstructors();
            IEnumerable<ConstructorInfo> b = objectType.GetTypeInfo().DeclaredConstructors;

            JToken jtoken = JToken.ReadFrom(reader);
            object converterSyncObj = this.converterSyncObj;
            try
            {
                lock (converterSyncObj)
                {
                    foreach (Type con in objectType.GenericTypeArguments)
                    {
                        foreach (var c in b)
                        {
                            try
                            {
                                Type key = con;
                                ((ICollection<JsonConverter>)serializer.Converters).Add(this.primitiveConverter);
                                object[] parameters = new object[1]
                                {
                                jtoken.ToObject(key, serializer)
                                };
                                return c.Invoke(parameters);
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
                }
            }
            finally
            {
            }
            throw new JsonSerializationException();
        }

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

        private sealed class XXX
        {
            public static readonly XXX Nine = new XXX();

            public XXX()
            {
            }

            internal Type Nine__3_0(ConstructorInfo info)
            {
                return ((IEnumerable<ParameterInfo>)info.GetParameters()).First<ParameterInfo>().ParameterType;
            }

            internal ConstructorInfo Nine__3_1(ConstructorInfo info)
            {
                return info;
            }

            internal Tuple<int, Type> Nine__3_2(Type value, int index)
            {
                return new Tuple<int, Type>(index, value);
            }
        }
    }
}
