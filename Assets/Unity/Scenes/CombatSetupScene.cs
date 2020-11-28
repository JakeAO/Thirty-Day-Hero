using Core.States.Combat;

namespace Unity.Scenes
{
    public class CombatSetupScene : SceneRootBase<CombatSetupState>
    {
        protected override void OnGUIContentForState()
        {
            //CombatSettings combatSettings = SharedContext.Get<CombatSettings>();
            //PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            //RenderEnemies(combatSettings != null ? combatSettings.Enemies : null);
            //RenderPartyCharacters(partyDataWrapper.Characters);
        }
    }
}