using System;
using Core.Classes.Enemy;
using Core.Database;
using Newtonsoft.Json;

namespace Core.JSON
{
    public class EnemyClassConverter : JsonConverter<IEnemyClass>
    {
        private readonly EnemyClassDatabase _enemyDatabase;
        private readonly CalamityClassDatabase _calamityDatabase;

        public EnemyClassConverter(EnemyClassDatabase enemyDatabase, CalamityClassDatabase calamityDatabase)
        {
            _enemyDatabase = enemyDatabase;
            _calamityDatabase = calamityDatabase;
        }

        public override void WriteJson(JsonWriter writer, IEnemyClass value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Id);
        }

        public override IEnemyClass ReadJson(JsonReader reader, Type objectType, IEnemyClass existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            uint id = Convert.ToUInt32(reader?.Value ?? 0);
            IEnemyClass item = _enemyDatabase.GetSpecific(id) ?? _calamityDatabase.GetSpecific(id);
            return item;
        }
    }
}