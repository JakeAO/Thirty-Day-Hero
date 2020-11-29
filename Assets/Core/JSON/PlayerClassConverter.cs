using System;
using Core.Classes.Player;
using Core.Database;
using Newtonsoft.Json;

namespace Core.JSON
{
    public class PlayerClassConverter : JsonConverter<IPlayerClass>
    {
        private readonly PlayerClassDatabase _database;

        public PlayerClassConverter(PlayerClassDatabase database)
        {
            _database = database;
        }

        public override void WriteJson(JsonWriter writer, IPlayerClass value, JsonSerializer serializer)
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

        public override IPlayerClass ReadJson(JsonReader reader, Type objectType, IPlayerClass existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                // We're reading the object itself.
                serializer.Converters.Remove(this);
                IPlayerClass item = (IPlayerClass) serializer.Deserialize(reader, objectType);
                serializer.Converters.Add(this);
                return item;
            }
            else
            {
                // We're reading a reference to the object.
                uint id = Convert.ToUInt32(reader?.Value ?? 0);
                IPlayerClass item = _database.GetSpecific(id);
                return item;
            }
        }
    }
}