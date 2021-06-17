using System;
using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Player;
using Core.EquipMap;
using Core.Etc;
using Core.States.Combat.Events;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.ActorChangeCalculator;
using SadPumpkin.Util.CombatEngine.Events;

namespace Core.States.Combat
{
    public class ActorChangeCalculator : IActorChangeCalculator
    {
        public IEnumerable<ICombatEventData> GetChangeEvents(IInitiativeActor before, IInitiativeActor after)
        {
            if (before.Id != after.Id)
                throw new ArgumentException("[ID MISMATCH] ActorChangeCalculator is meant for changes between old and new versions of the SAME actor!");

            // Life
            foreach (ICombatEventData eventData in GetLifeDiffEvents(before, after))
            {
                yield return eventData;
            }

            // Party
            foreach (ICombatEventData eventData in GetPartyDiffEvents(before, after))
            {
                yield return eventData;
            }

            // Stats
            if (before is ICharacterActor beforeChar &&
                after is ICharacterActor afterChar)
            {
                foreach (ICombatEventData eventData in GetStatMapDiffEvents(afterChar.Id, beforeChar.Stats, afterChar.Stats))
                {
                    yield return eventData;
                }
            }

            // Equipment
            if (before is IPlayerCharacterActor beforePC &&
                after is IPlayerCharacterActor afterPC)
            {
                foreach (ICombatEventData eventData in GetEquipMapDiffEvents(afterPC.Id, beforePC.Equipment, afterPC.Equipment))
                {
                    yield return eventData;
                }
            }
        }

        private IEnumerable<ICombatEventData> GetLifeDiffEvents(IInitiativeActor before, IInitiativeActor after)
        {
            bool beforeLife = before.IsAlive();
            bool afterLife = after.IsAlive();

            if (beforeLife == afterLife)
                yield break;

            if (beforeLife)
            {
                yield return new ActorDiedEvent(before.Id);
            }
            else
            {
                yield return new ActorResurrectedEvent(after.Id);
            }
        }

        private IEnumerable<ICombatEventData> GetPartyDiffEvents(IInitiativeActor before, IInitiativeActor after)
        {
            uint beforeParty = before.Party;
            uint afterParty = after.Party;

            if (beforeParty == afterParty)
                yield break;

            yield return new PartyChangedEvent(before.Id, before.Party, after.Party);
        }

        private IEnumerable<ICombatEventData> GetStatMapDiffEvents(uint actorId, IStatMap before, IStatMap after)
        {
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                uint oldStat = before[statType];
                uint newStat = after[statType];

                if (oldStat != newStat)
                {
                    yield return new StatChangedEvent(actorId, statType, oldStat, newStat);
                }
            }
        }

        private IEnumerable<ICombatEventData> GetEquipMapDiffEvents(uint actorId, IEquipMap before, IEquipMap after)
        {
            foreach (EquipmentSlot equipmentSlot in Enum.GetValues(typeof(EquipmentSlot)))
            {
                uint oldItemId = before[equipmentSlot]?.Id ?? 0;
                uint newItemId = after[equipmentSlot]?.Id ?? 0;

                if (oldItemId != newItemId)
                {
                    yield return new EquipmentChangedEvent(actorId, equipmentSlot, oldItemId, newItemId);
                }
            }
        }
    }
}