using System;
using Core.Classes.Calamity;
using Core.Database;
using Newtonsoft.Json;

namespace Core.JSON
{
    public class CalamityClassConverter : JsonConverter<ICalamityClass>
    {
        private readonly CalamityClassDatabase _calamityDatabase;

        public CalamityClassConverter(CalamityClassDatabase calamityDatabase)
        {
            _calamityDatabase = calamityDatabase;
        }

        public override void WriteJson(JsonWriter writer, ICalamityClass value, JsonSerializer serializer)
        {
            if (writer.WriteState == WriteState.Start)
            {
                // We're writing the object itself.
                serializer.Converters.Remove(this);
                serializer.Serialize(writer, value);
                serializer.Converters.Add(this);
            }
            else
            {
                // We're writing a reference to the object.
                writer.WriteValue(value.Id);
            }
        }

        public override ICalamityClass ReadJson(JsonReader reader, Type objectType, ICalamityClass existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                // We're reading the object itself.
                serializer.Converters.Remove(this);
                ICalamityClass item = (ICalamityClass) serializer.Deserialize(reader, objectType);
                serializer.Converters.Add(this);
                return item;
            }
            else
            {
                // We're reading a reference to the object.
                uint id = Convert.ToUInt32(reader?.Value ?? 0);
                return _calamityDatabase.GetSpecific(id);
            }
        }
    }
}