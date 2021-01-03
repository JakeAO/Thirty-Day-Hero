using System;
using System.Collections.Generic;
using Core.Actors.Player;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class RestState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";
        
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

        public PartyDataWrapper PartyData;
        public TimeOfDay TimeOfDay;
        public IReadOnlyDictionary<uint, (uint hp, uint sta)> RestoredStats;

        private RestProvider _restProvider = null;

        public override void OnEnter(IState fromState)
        {
            PartyData = SharedContext.Get<PartyDataWrapper>();
            _restProvider = PartyData.Time == TimeOfDay.Night
                ? new RestProvider(0.5, 0.5)
                : new RestProvider(0.2, 0.2);

            TimeOfDay = PartyData.Time;
        }

        public override void OnContent()
        {
            _restProvider.RestParty(PartyData, out RestoredStats);

            _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>()
            {
                new EventOption(
                    "Continue",
                    GoToGameHub)
            };
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }
    }
}