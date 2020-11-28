using System.Collections.Generic;
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
                GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(150), GUILayout.Height(60));
                {
                    GUILayout.Label($"Name: {actor.Name}");
                    GUILayout.Label($"Class: {actor.Class.Name} ({actor.Class.Desc})");

                    result = GUILayout.Button("Toggle Hero");
                }
                GUILayout.EndVertical();
                return result;
            }

            GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(620));
            {
                GUILayout.Label("Debug flow:");

                uint selectedActorId = 0u;

                GUILayout.Label("Calamity", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                GUILayout.Label($"Name: {_state.PartyData.Calamity.Name}");
                GUILayout.Label($"Class: {_state.PartyData.Calamity.Class} ({_state.PartyData.Calamity.Class.Desc})");
                
                GUILayout.Label($"Party ({_state.PartyData.Characters.Count}/{Constants.PARTY_SIZE_MAX})", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                List<PlayerCharacter> party = _state.PartyData.Characters;
                if (party.Count > 0)
                {
                    GUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        foreach (PlayerCharacter actorInParty in party)
                        {
                            if (DrawActorBox(actorInParty))
                                selectedActorId = actorInParty.Id;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(70));
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("No Party Characters");
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndVertical();
                }

                GUILayout.Label($"Hero Pool", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.Height(65));
                {
                    foreach (PlayerCharacter actorInPool in _state.UnassignedCharacterPool)
                    {
                        if (DrawActorBox(actorInPool))
                            selectedActorId = actorInPool.Id;
                    }
                }
                GUILayout.EndHorizontal();
                

                if (selectedActorId != 0u)
                {
                    _state.SelectActorById(selectedActorId);
                }

                bool canSubmit = _state.PartyData.Characters.Count >= Constants.PARTY_SIZE_MIN &&
                                 _state.PartyData.Characters.Count <= Constants.PARTY_SIZE_MAX;


                GUI.enabled = canSubmit;
                GUI.color = canSubmit ? Color.green : Color.white;
                if (GUILayout.Button("Submit Party", GUILayout.Height(40)))
                {
                    _state.SubmitParty();
                }
                GUI.enabled = true;
                GUI.color = Color.white;
            }
            GUILayout.EndVertical();
        }
    }
}