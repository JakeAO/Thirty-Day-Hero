using System;
using Core.Classes.Enemy;
using Core.Database;
using Newtonsoft.Json;

namespace Core.JSON
{
    public class EnemyClassConverter : JsonConverter<IEnemyClass>
    {
        private readonly EnemyClassDatabase _enemyDatabase;

        public EnemyClassConverter(EnemyClassDatabase enemyDatabase)
        {
            _enemyDatabase = enemyDatabase;
        }

        public override void WriteJson(JsonWriter writer, IEnemyClass value, JsonSerializer serializer)
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

        public override IEnemyClass ReadJson(JsonReader reader, Type objectType, IEnemyClass existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                // We're reading the object itself.
                serializer.Converters.Remove(this);
                IEnemyClass item = (IEnemyClass) serializer.Deserialize(reader, objectType);
                serializer.Converters.Add(this);
                return item;
            }
            else
            {
                // We're reading a reference to the object.
                uint id = Convert.ToUInt32(reader?.Value ?? 0);
                return _enemyDatabase.GetSpecific(id);
            }
        }
    }
}