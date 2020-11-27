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
            writer.WriteValue(value.Id);
        }

        public override IPlayerClass ReadJson(JsonReader reader, Type objectType, IPlayerClass existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            uint id = Convert.ToUInt32(reader?.Value ?? 0);
            IPlayerClass item = _database.GetSpecific(id);
            return item;
        }
    }
}