using System;
using Core.Database;
using Core.Items.Weapons;
using Newtonsoft.Json;

namespace Core.JSON
{
    public class WeaponJsonConverter : JsonConverter<IWeapon>
    {
        private readonly WeaponDatabase _database;

        public WeaponJsonConverter(WeaponDatabase database)
        {
            _database = database;
        }

        public override void WriteJson(JsonWriter writer, IWeapon value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Id);
        }

        public override IWeapon ReadJson(JsonReader reader, Type objectType, IWeapon existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            uint id = Convert.ToUInt32(reader?.Value ?? 0);
            IWeapon item = _database.GetSpecific(id);
            return item;
        }
    }
}