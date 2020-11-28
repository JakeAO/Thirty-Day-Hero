using System.Collections.Generic;
using Core.Etc;
using Core.EventOptions;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;

namespace Core.States
{
    public class VictoryState : TDHStateBase
    {
        public override void OnContent()
        {
            PlayerDataWrapper playerDataWrapper = SharedContext.Get<PlayerDataWrapper>();
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();
            
            // Update and Save Party Data
            partyDataWrapper.CalamityDefeated = true;
            SaveLoadHelper.SavePartyData(SharedContext);

            // Update and Save Player Data
            playerDataWrapper.SetActiveParty(0u);
            SaveLoadHelper.SavePlayerData(SharedContext);
            
            // Clear Party Data
            SharedContext.Clear<PartyDataWrapper>();
        }

        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption(
                "New Party",
                GoToCreateParty);
        }

        private void GoToCreateParty()
        {
            SharedContext.Get<IStateMachine>().ChangeState<CreatePartyState>();
        }
    }
}