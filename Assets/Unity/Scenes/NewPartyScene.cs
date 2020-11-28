using System.IO;
using Core.Actors.Player;
using Core.Etc;
using Core.States;
using SadPumpkin.Util.StateMachine.States;
using UnityEngine;

namespace Unity.Scenes
{
    public class NewPartyScene : SceneRootBase
    {
        private CreatePartyState _state = null;

        protected override void OnInject()
        {
            base.OnInject();

            _state = Context.Get<IState>() as CreatePartyState;

            if (_state == null)
                throw new InvalidDataException($"NewPartyScene expects CreatePartyState but was injected with {_state}");
        }

        private void OnGUI()
        {
            bool DrawActorBox(IPlayerCharacterActor actor)
            {
                bool result;
                GUILayout.BeginVertical(GUI.skin.box);
                {
                    GUILayout.Label($"Name: {actor.Name}");
                    GUILayout.Label($"Class: {actor.Class.Name} ({actor.Class.Desc})");

                    result = GUILayout.Button("Toggle Hero");
                }
                GUILayout.EndVertical();
                return result;
            }

            GUILayout.BeginArea(new Rect(10, 10, 150, 150), GUI.skin.box);
            {
                GUILayout.Label("Debug flow:");

                uint selectedActorId = 0u;

                GUILayout.Label("Calamity", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                GUILayout.Label($"Name: {_state.PartyData.Calamity.Name}");
                GUILayout.Label($"Class: {_state.PartyData.Calamity.Class} ({_state.PartyData.Calamity.Class.Desc})");
                
                GUILayout.Label($"Party ({_state.PartyData.Characters.Count}/{Constants.PARTY_SIZE_MAX})", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                foreach (PlayerCharacter actorInParty in _state.PartyData.Characters)
                {
                    if (DrawActorBox(actorInParty))
                        selectedActorId = actorInParty.Id;
                }

                GUILayout.Label($"Hero Pool", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                foreach (PlayerCharacter actorInPool in _state.UnassignedCharacterPool)
                {
                    if (DrawActorBox(actorInPool))
                        selectedActorId = actorInPool.Id;
                }

                if (selectedActorId != 0u)
                {
                    _state.SelectActorById(selectedActorId);
                }

                GUI.enabled = _state.PartyData.Characters.Count >= Constants.PARTY_SIZE_MIN &&
                              _state.PartyData.Characters.Count <= Constants.PARTY_SIZE_MAX;
                if (GUILayout.Button("Submit Party"))
                {
                    _state.SubmitParty();
                }
                GUI.enabled = true;
            }
            GUILayout.EndArea();
        }
    }
}