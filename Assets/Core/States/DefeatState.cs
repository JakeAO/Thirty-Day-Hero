using System.Collections.Generic;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;

namespace Core.States
{
    public class DefeatState : TDHStateBase
    {
        public const string CATEGORY_CONTINUE = "Continue";

        public override void OnContent()
        {
            PlayerDataWrapper playerDataWrapper = SharedContext.Get<PlayerDataWrapper>();
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            // Update and Save Party Data
            partyDataWrapper.CalamityDefeated = false;
            SaveLoadHelper.SavePartyData(SharedContext);

            // Update and Save Player Data
            playerDataWrapper.SetActiveParty(0u);
            SaveLoadHelper.SavePlayerData(SharedContext);

            // Clear Party Data
            SharedContext.Clear<PartyDataWrapper>();

            _currentOptions[CATEGORY_CONTINUE] = new List<IEventOption>()
            {
                new EventOption(
                    "New Party",
                    GoToCreateParty,
                    CATEGORY_CONTINUE)
            };
        }

        private void GoToCreateParty()
        {
            SharedContext.Get<IStateMachine>().ChangeState<CreatePartyState>();
        }
    }
}