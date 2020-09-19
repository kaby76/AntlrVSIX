using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Protocol
{
    public class ParameterInformationConverter : JsonConverter
    {
        public ParameterInformationConverter() { }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jtoken1 = JToken.Load(reader);
            JProperty jproperty1 = ((JObject)jtoken1).Property("label");
            JProperty jproperty2 = ((JObject)jtoken1).Property("documentation");
            ParameterInformation parameterInformation = new ParameterInformation();
            if (jproperty1 != null)
            {
                JToken jtoken2 = jproperty1.Value;
                if (jtoken2 is JArray jarray)
                {
                    Tuple<int, int> tuple = new Tuple<int, int>((int)Extensions.Value<int>((IEnumerable<JToken>)jarray[0]), (int)Extensions.Value<int>((IEnumerable<JToken>)jarray[1]));
                    parameterInformation.Label = (SumType<string, Tuple<int, int>>)tuple;
                }
                else
                    parameterInformation.Label = (SumType<string, Tuple<int, int>>)jtoken2.ToObject<SumType<string, Tuple<int, int>>>();
            }
            if (jproperty2 != null)
            {
                JToken jtoken2 = jproperty2.Value;
                parameterInformation.Documentation = (SumType<string, MarkupContent>)jtoken2.ToObject<SumType<string, MarkupContent>>();
            }
            return (object)parameterInformation;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
