using System;
using Core.Database;
using Core.Items.Armors;
using Newtonsoft.Json;

namespace Core.JSON
{
    public class ArmorJsonConverter : JsonConverter<IArmor>
    {
        private readonly ArmorDatabase _database;

        public ArmorJsonConverter(ArmorDatabase database)
        {
            _database = database;
        }

        public override void WriteJson(JsonWriter writer, IArmor value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Id);
        }

        public override IArmor ReadJson(JsonReader reader, Type objectType, IArmor existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            uint id = Convert.ToUInt32(reader?.Value ?? 0);
            IArmor item = _database.GetSpecific(id);
            return item;
        }
    }
}