using System;

using Core.States;
using Core.States.Town;

using SadPumpkin.Util.StateMachine.Signals;
using SadPumpkin.Util.StateMachine.States;
using SadPumpkin.Util.Context;

using UnityEngine.SceneManagement;

namespace Unity.Scenes
{
    public class SceneController : IDisposable
    {
        private const string SCENE_NAME_NEW_PARTY = "NewParty";
        private const string SCENE_NAME_PRE_GAME = "PreGame";
        private const string SCENE_NAME_GAME_HUB = "GameHub";
        private const string SCENE_NAME_TOWN_HUB = "Town";

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
        }

        private void OnStateChanged(IState newState)
        {
            string sceneName = SceneNameForState(newState);
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        private string SceneNameForState(IState state)
        {
            switch (state)
            {
                case CreatePartyState _:
                    return SCENE_NAME_NEW_PARTY;
                case PreGameState _:
                    return SCENE_NAME_PRE_GAME;
                case GameHubState _:
                    return SCENE_NAME_GAME_HUB;
                case TownHubState _:
                    return SCENE_NAME_TOWN_HUB;
                default:
                    return string.Empty;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.Equals(default(Scene)))
                return;

            SceneRootBase sceneRoot = scene.GetSceneComponent<SceneRootBase>();
            if (sceneRoot == null)
                return;

            sceneRoot.InjectContext(_context);
        }
    }
}