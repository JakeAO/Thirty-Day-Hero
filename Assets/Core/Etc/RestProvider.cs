using System;
using System.Collections.Generic;
using Core.Actors.Player;
using Core.Wrappers;

namespace Core.Etc
{
    public class RestProvider
    {
        private double _hpRestore;
        private double _staRestore;

        public RestProvider(double hpRestorationPercent, double staRestorationPercent)
        {
            _hpRestore = hpRestorationPercent;
            _staRestore = staRestorationPercent;
        }

        public void RestParty(PartyDataWrapper partyDataWrapper, out IReadOnlyDictionary<uint, (uint hp, uint sta)> restoredStatsByActor)
        {
            Dictionary<uint, (uint hp, uint sta)> restoredStats = new Dictionary<uint, (uint hp, uint sta)>(partyDataWrapper.Characters.Count);
            restoredStatsByActor = restoredStats;

            foreach (PlayerCharacter playerCharacter in partyDataWrapper.Characters)
            {
                if (!playerCharacter.IsAlive())
                    continue;

                uint hpBoost = (uint) Math.Round(playerCharacter.Stats[StatType.HP_Max] * _hpRestore);
                uint staBoost = (uint) Math.Round(playerCharacter.Stats[StatType.STA_Max] * _staRestore);

                hpBoost = Math.Min(hpBoost, playerCharacter.Stats[StatType.HP_Max] - playerCharacter.Stats[StatType.HP]);
                staBoost = Math.Min(staBoost, playerCharacter.Stats[StatType.STA_Max] - playerCharacter.Stats[StatType.STA]);

                playerCharacter.Stats.ModifyStat(StatType.HP, (int) hpBoost);
                playerCharacter.Stats.ModifyStat(StatType.STA, (int) staBoost);

                restoredStats[playerCharacter.Id] = (hpBoost, staBoost);
            }
        }
    }
}