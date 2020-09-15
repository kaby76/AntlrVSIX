using Newtonsoft.Json;
using System;

namespace Protocol
{
    internal class StrictPrimitiveConverter : JsonConverter
    {
        private JsonSerializer defaultSerializer;

        /// <inheritdoc />
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return StrictPrimitiveConverter.IsFloat(objectType) || StrictPrimitiveConverter.IsBool(objectType) || StrictPrimitiveConverter.IsInteger(objectType) || StrictPrimitiveConverter.IsString(objectType);
        }

        /// <inheritdoc />
        public override object ReadJson(
          JsonReader reader,
          Type objectType,
          object existingValue,
          JsonSerializer serializer)
        {
            switch ((int)reader.TokenType - 7)
            {
                case 0:
                    if (!StrictPrimitiveConverter.IsInteger(objectType))
                        throw new InvalidCastException();
                    return this.defaultSerializer.Deserialize(reader, objectType);
                case 1:
                    if (!StrictPrimitiveConverter.IsFloat(objectType))
                        throw new InvalidCastException();
                    return this.defaultSerializer.Deserialize(reader, objectType);
                case 2:
                case 4:
                    if (!StrictPrimitiveConverter.IsString(objectType))
                        throw new InvalidCastException();
                    return this.defaultSerializer.Deserialize(reader, objectType);
                case 3:
                    if (!StrictPrimitiveConverter.IsBool(objectType))
                        throw new InvalidCastException();
                    return this.defaultSerializer.Deserialize(reader, objectType);
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private static bool IsFloat(Type type)
        {
            return type == typeof(double) || type == typeof(float);
        }

        private static bool IsBool(Type type)
        {
            return type == typeof(bool);
        }

        private static bool IsInteger(Type type)
        {
            return type == typeof(int) || type == typeof(uint) || (type == typeof(long) || type == typeof(ulong)) || (type == typeof(short) || type == typeof(ushort) || type == typeof(byte)) || type == typeof(sbyte);
        }

        private static bool IsString(Type type)
        {
            return type == typeof(string);
        }

        public StrictPrimitiveConverter()
            : base()
        {
            this.defaultSerializer = new JsonSerializer();
        }
    }
}
