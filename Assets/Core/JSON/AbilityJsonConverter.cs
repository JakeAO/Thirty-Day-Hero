using System;
using Core.Abilities;
using Core.Database;
using Newtonsoft.Json;

namespace Core.JSON
{
    public class AbilityJsonConverter : JsonConverter<IAbility>
    {
        private readonly AbilityDatabase _database;

        public AbilityJsonConverter(AbilityDatabase database)
        {
            _database = database;
        }

        public override void WriteJson(JsonWriter writer, IAbility value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Id);
        }

        public override IAbility ReadJson(JsonReader reader, Type objectType, IAbility existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            uint id = Convert.ToUInt32(reader?.Value ?? 0);
            IAbility item = _database.GetSpecific(id);
            return item;
        }
    }
}