using System;
using System.Collections.Generic;
using Core.CombatSettings;
using Core.Database;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.States.Combat;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;

namespace Core.States
{
    public class PatrolState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";
        
        public EnemyGroupWrapper EnemyGroup { get; private set; }
        public CombatDifficulty Difficulty { get; private set; }
        public CombatSettings.CombatSettings Settings { get; private set; }

        public override void OnContent()
        {
            // TODO some kind of weighted thing
            int random = new Random().Next(10);
            if (random < 3)
                Difficulty = CombatDifficulty.Easy;
            else if (random > 7)
                Difficulty = CombatDifficulty.Hard;
            else
                Difficulty = CombatDifficulty.Normal;

            EnemyGroup = SharedContext.Get<EnemyGroupWrapperDatabase>().GetRandom();
            Settings = SharedContext.Get<CombatSettingsGenerator>().CreateFromEnemyGroup(
                EnemyGroup,
                Difficulty,
                SharedContext.Get<PartyDataWrapper>());

            _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>
            {
                new EventOption(
                    "Fight",
                    GoToCombat,
                    priority: 0),
                new EventOption(
                    "Run",
                    GoToGameHub,
                    priority: 1)
            };
        }

        private void GoToCombat()
        {
            SharedContext.Get<IStateMachine>().ChangeState(
                new CombatState(Settings));
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }
    }
}