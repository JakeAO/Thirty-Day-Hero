using System.Collections.Generic;
using Core.EventOptions;
using SadPumpkin.Util.StateMachine;

namespace Core.States.Town
{
    public class TownHubState : TDHStateBase
    {
        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption(
                "Leave Town",
                GoToGameHub,
                priority: 99);
            yield return new EventOption(
                "Enter Item Shop",
                GoToItemShop,
                priority: 0);
            yield return new EventOption(
                "Enter Armor Shop",
                GoToArmorShop,
                priority: 1);
            yield return new EventOption(
                "Enter Weapon Shop",
                GoToWeaponShop,
                priority: 2);
            yield return new EventOption(
                "Enter Dojo",
                GoToDojo,
                priority: 4);
            yield return new EventOption(
                "Enter Inn",
                GoToInn,
                priority: 5);
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }

        private void GoToItemShop()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownShopState>();
        }

        private void GoToArmorShop()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownShopState>();
        }

        private void GoToWeaponShop()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownShopState>();
        }

        private void GoToDojo()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownDojoState>();
        }

        private void GoToInn()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownInnState>();
        }
    }
}