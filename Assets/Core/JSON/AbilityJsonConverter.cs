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

        public override IAbility ReadJson(JsonReader reader, Type objectType, IAbility existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                // We're reading the object itself.
                serializer.Converters.Remove(this);
                IAbility item = (IAbility) serializer.Deserialize(reader, objectType);
                serializer.Converters.Add(this);
                return item;
            }
            else
            {
                // We're reading a reference to the object.
                uint id = Convert.ToUInt32(reader?.Value ?? 0);
                IAbility item = _database.GetSpecific(id);
                return item;
            }
        }
    }
}