using System;
using System.Collections.Generic;
using Core.Actors.Player;
using Core.EquipMap;
using Core.Etc;
using Core.EventOptions;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;
using Core.States.BaseClasses;
using Core.Wrappers;
using SadPumpkin.Util.Context;

namespace Core.States.SubStates
{
    public class EquipmentSubState : TDHSubStateBase
    {
        public bool Active;
        
        private readonly IReadOnlyList<PlayerCharacter> _party;
        private readonly List<IItem> _inventory;

        public EquipmentSubState(IContext sharedContext)
        {
            SharedContext = sharedContext;

            var partyDataWrapper = sharedContext.Get<PartyDataWrapper>();

            _party = partyDataWrapper.Characters;
            _inventory = partyDataWrapper.Inventory;

            SetupOptions();
        }

        private void SetupOptions()
        {
            foreach (PlayerCharacter character in _party)
            {
                string key = $"{character.Name} ({character.Id})";
                if (!_currentOptions.TryGetValue(key, out var optionList))
                    _currentOptions[key] = optionList = new List<IEventOption>();
                optionList.Clear();

                if (character.Equipment is EquipMap.EquipMap mutableEquipMap)
                {
                    // Unequip Equipped Items
                    foreach (EquipmentSlot equipmentSlot in Enum.GetValues(typeof(EquipmentSlot)))
                    {
                        IItem item = mutableEquipMap[equipmentSlot];
                        if (item != null)
                        {
                            optionList.Add(new EventOption(
                                $"{item.Name} (Remove)",
                                () => UnequipItem(mutableEquipMap, item, equipmentSlot),
                                character.Name,
                                0,
                                context: item));
                        }
                    }

                    // Equip Inventory Items
                    foreach (IItem inventoryItem in _inventory)
                    {
                        switch (inventoryItem.ItemType)
                        {
                            case ItemType.Weapon:
                            {
                                if (inventoryItem is IWeapon inventoryWeapon)
                                {
                                    if (character.Class.WeaponProficiency.HasFlag(inventoryWeapon.WeaponType))
                                    {
                                        optionList.Add(new EventOption(
                                            $"{inventoryItem.Name} (Equip Weapon)",
                                            () => EquipItem(mutableEquipMap, inventoryItem, EquipmentSlot.Weapon),
                                            character.Name,
                                            1,
                                            context: inventoryItem));
                                    }
                                }

                                break;
                            }
                            case ItemType.Armor:
                            {
                                if (inventoryItem is IArmor inventoryArmor)
                                {
                                    if (character.Class.ArmorProficiency.HasFlag(inventoryArmor.ArmorType))
                                    {
                                        optionList.Add(new EventOption(
                                            $"{inventoryItem.Name} (Equip Armor)",
                                            () => EquipItem(mutableEquipMap, inventoryItem, EquipmentSlot.Armor),
                                            character.Name,
                                            2,
                                            context: inventoryItem));
                                    }
                                }

                                break;
                            }
                            case ItemType.Consumable:
                            case ItemType.Trinket:
                            {
                                optionList.Add(new EventOption(
                                    $"{inventoryItem.Name} (Equip Item A)",
                                    () => EquipItem(mutableEquipMap, inventoryItem, EquipmentSlot.ItemA),
                                    character.Name,
                                    3,
                                    context: inventoryItem));
                                optionList.Add(new EventOption(
                                    $"{inventoryItem.Name} (Equip Item B)",
                                    () => EquipItem(mutableEquipMap, inventoryItem, EquipmentSlot.ItemB),
                                    character.Name,
                                    3,
                                    context: inventoryItem));
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void UnequipItem(EquipMap.EquipMap equipMap, IItem item, EquipmentSlot slot)
        {
            if (equipMap != null &&
                item != null &&
                equipMap[slot] == item)
            {
                equipMap[slot] = null;

                _inventory.Add(item);

                SetupOptions();
            }
        }

        private void EquipItem(EquipMap.EquipMap equipMap, IItem item, EquipmentSlot slot)
        {
            if (equipMap != null &&
                item != null)
            {
                IItem equippedItem = equipMap[slot];
                if (equippedItem != null)
                    _inventory.Add(equippedItem);

                _inventory.Remove(item);
                equipMap[slot] = item;

                SetupOptions();
            }
        }
    }
}