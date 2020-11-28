using Core.Actors.Player;
using Core.Etc;
using Core.States;
using UnityEngine;

namespace Unity.Scenes
{
    public class NewPartyScene : SceneRootBase<CreatePartyState>
    {
        protected override void OnGUIContentForState()
        {
            GUILayout.Label("Calamity", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
            GUILayout.Label($"Name: {State.PartyData.Calamity.Name}");
            GUILayout.Label($"Class: {State.PartyData.Calamity.Class.Name} ({State.PartyData.Calamity.Class.Desc})");
            GUILayout.Space(10);
            GUILayout.Label($"Party Size: {State.PartyData.Characters.Count}/{Constants.PARTY_SIZE_MAX}");
        }

        protected override void OnGUIContentForContext(object context)
        {
            switch (context)
            {
                case IPlayerCharacterActor actor:
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        GUILayout.Label($"Name: {actor.Name}");
                        GUILayout.Label($"Class: {actor.Class.Name} ({actor.Class.Desc})");
                    }
                    GUILayout.EndVertical();
                    break;
                }
            }
        }
    }
}