using System;
using Core.EquipMap;
using Core.Items;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using IPlayerCharacterActor = Core.Actors.Player.IPlayerCharacterActor;

namespace Core.Costs
{
    public class DestroyThisItemCost : ICostCalc
    {
        public bool CanAfford(IInitiativeActor entity, IIdTracked actionSource)
        {
            if (entity is IPlayerCharacterActor playerCharacter &&
                actionSource is IItem itemActionSource)
            {
                foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
                {
                    if (playerCharacter.Equipment[slot] is IItem item)
                    {
                        if (item.Id == itemActionSource.Id)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void Pay(IInitiativeActor entity, IIdTracked actionSource)
        {
            if (entity is IPlayerCharacterActor playerCharacter &&
                playerCharacter.Equipment is EquipMap.EquipMap rawEquipMap &&
                actionSource is IItem itemActionSource)
            {
                foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
                {
                    if (rawEquipMap[slot] is IItem item)
                    {
                        if (item.Id == itemActionSource.Id)
                        {
                            rawEquipMap[slot] = null;
                        }
                    }
                }
            }
        }

        public string Description()
        {
            return "Consumes Item";
        }
    }
}