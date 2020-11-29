using System;
using Core.Database;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;
using Newtonsoft.Json;

namespace Core.JSON
{
    public class ItemJsonConverter : JsonConverter<IItem>
    {
        private readonly ItemDatabase _itemDatabase;
        private readonly WeaponDatabase _weaponDatabase;
        private readonly ArmorDatabase _armorDatabase;

        public ItemJsonConverter(ItemDatabase itemDatabase, WeaponDatabase weaponDatabase, ArmorDatabase armorDatabase)
        {
            _itemDatabase = itemDatabase;
            _weaponDatabase = weaponDatabase;
            _armorDatabase = armorDatabase;
        }

        public override void WriteJson(JsonWriter writer, IItem value, JsonSerializer serializer)
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

        public override IItem ReadJson(JsonReader reader, Type objectType, IItem existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                // We're reading the object itself.
                serializer.Converters.Remove(this);
                IItem item = (IItem) serializer.Deserialize(reader, objectType);
                serializer.Converters.Add(this);
                return item;
            }
            else
            {
                // We're reading a reference to the object.
                uint id = Convert.ToUInt32(reader?.Value ?? 0);

                if (typeof(IWeapon).IsAssignableFrom(objectType))
                {
                    return _weaponDatabase.GetSpecific(id);
                }

                if (typeof(IArmor).IsAssignableFrom(objectType))
                {
                    return _armorDatabase.GetSpecific(id);
                }

                return _itemDatabase.GetSpecific(id);
            }
        }
    }
}