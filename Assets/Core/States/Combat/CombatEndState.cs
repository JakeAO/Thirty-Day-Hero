using System;
using System.Collections.Generic;
using System.Linq;
using Core.Actors.Player;
using Core.Classes.Calamity;
using Core.CombatSettings;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.StatMap;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Combat
{
    public class CombatEndState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";
        public const string CATEGORY_DEBUG = "";

        public bool Victory { get; private set; }
        public uint GoldReward { get; private set; }
        public uint ExpPerCharacter { get; private set; }
        public IReadOnlyDictionary<uint, int[]> StatChanges => _statChanges;

        private CombatSettings.CombatSettings _settings;
        private CombatResults _results;

        private readonly Dictionary<uint, int[]> _statChanges = new Dictionary<uint, int[]>(3);

        public CombatEndState(
            CombatSettings.CombatSettings settings,
            CombatResults results)
        {
            _settings = settings;
            _results = results;
        }

        public override void OnEnter(IState fromState)
        {
            PartyDataWrapper partyData = SharedContext.Get<PartyDataWrapper>();

            if (_results.Success)
            {
                Victory = true;
                GoldReward = _results.GoldReward;
                ExpPerCharacter = (uint) Math.Ceiling(_results.ExpReward / (float) partyData.Characters.Count);

                Random random = new Random();
                foreach (PlayerCharacter playerCharacter in partyData.Characters)
                {
                    IStatMap originalStats = playerCharacter.Stats.Copy();

                    uint level = playerCharacter.Stats[StatType.LVL];
                    playerCharacter.Stats.ModifyStat(StatType.EXP, (int) ExpPerCharacter);
                    uint newLevel = playerCharacter.Stats[StatType.LVL];
                    while (level < newLevel)
                    {
                        playerCharacter.Stats = playerCharacter.Class.LevelUpStats.Increment(playerCharacter.Stats, random);
                        level++;
                    }

                    _statChanges[playerCharacter.Id] = GetDiffs(playerCharacter.Stats, originalStats);
                }
            }
        }

        public override void OnContent()
        {
            _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>()
            {
                new EventOption(
                    "Continue",
                    OnContinue)
            };
        }

        private void OnContinue()
        {
            if (_results.Success)
            {
                if (_settings.Enemies.Count == 1 &&
                    _settings.Enemies.First().Class is ICalamityClass)
                {
                    SharedContext.Get<IStateMachine>().ChangeState<VictoryState>();
                }
                else
                {
                    SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
                }
            }
            else
            {
                SharedContext.Get<IStateMachine>().ChangeState<DefeatState>();
            }
        }

        private static int[] GetDiffs(IStatMap newMap, IStatMap oldMap)
        {
            int[] diffs = new int[Enum.GetValues(typeof(StatType)).Length];
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                diffs[(int) statType] = Math.Max(0, (int) newMap[statType] - (int) oldMap[statType]);
            }

            return diffs;
        }
    }
}