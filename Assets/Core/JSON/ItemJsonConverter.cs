using System;
using Core.Database;
using Core.Items;
using Newtonsoft.Json;

namespace Core.JSON
{
    public class ItemJsonConverter : JsonConverter<IItem>
    {
        private readonly ItemDatabase _database;

        public ItemJsonConverter(ItemDatabase database)
        {
            _database = database;
        }

        public override void WriteJson(JsonWriter writer, IItem value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Id);
        }

        public override IItem ReadJson(JsonReader reader, Type objectType, IItem existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            uint id = Convert.ToUInt32(reader?.Value ?? 0);
            IItem item = _database.GetSpecific(id);
            return item;
        }
    }
}