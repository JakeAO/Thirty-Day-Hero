using System;
using System.Collections.Generic;
using Core.States;
using Core.States.Combat;
using Core.States.Town;

using SadPumpkin.Util.StateMachine.Signals;
using SadPumpkin.Util.StateMachine.States;
using SadPumpkin.Util.Context;

using UnityEngine.SceneManagement;

namespace Unity.Scenes
{
    public class SceneController : IDisposable
    {
        private static readonly Dictionary<Type, string> StatesToSceneNames = new Dictionary<Type, string>()
        {
            {typeof(StartupState), "Boot"},
            {typeof(PreGameState), "PreGame"},
            {typeof(CreatePartyState), "NewParty"},
            {typeof(GameHubState), "GameHub"},
            {typeof(RestState), "Rest"},
            {typeof(PatrolState), "Patrol"},
            {typeof(EncounterState), "Encounter"},
            {typeof(TownHubState), "Town"},
            {typeof(TownInnState), "TownInn"},
            {typeof(TownShopState), "TownShop"},
            {typeof(TownDojoState), "TownDojo"},
            {typeof(CombatSetupState), "CombatSetup"},
            {typeof(CombatMainState), "Combat"},
            {typeof(CombatEndState), "CombatResults"},
            {typeof(VictoryState), "Victory"},
            {typeof(DefeatState), "Defeat"},
        };

        private readonly IContext _context = null;

        public SceneController(IContext context)
        {
            _context = context;
            _context.Get<StateChanged>().Listen(OnStateChanged);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void Dispose()
        {
            _context?.Get<StateChanged>()?.Unlisten(OnStateChanged);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnStateChanged(IState newState)
        {
            StatesToSceneNames.TryGetValue(newState.GetType(), out string sceneName);
            SceneManager.LoadScene(sceneName);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.Equals(default(Scene)))
                return;

            ISceneRoot sceneRoot = scene.GetSceneComponent<ISceneRoot>();

            sceneRoot?.InjectContext(_context);
        }
    }
}