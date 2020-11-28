using Core.States.Town;
using Core.Wrappers;
using UnityEngine;

namespace Unity.Scenes
{
    public class TownScene : SceneRootBase<TownHubState>
    {
        protected override void OnGUIContentForState()
        {
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            GUILayout.Label($"Day: {partyDataWrapper.Day} ({partyDataWrapper.Time})");
            GUILayout.Label($"Gold: {partyDataWrapper.Gold}");
        }
    }
}
