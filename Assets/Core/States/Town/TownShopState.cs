using System.Collections.Generic;
using Core.EventOptions;
using Core.Items;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Town
{
    public class TownShopState : TDHStateBase
    {
        public readonly string ShopName = null;
        
        private readonly IList<IItem> _shopInventory = null;
        private readonly IList<IItem> _partyInventory = null;

        private float _buyPriceMultiplier = 1f;
        private float _sellPriceMultiplier = 0.5f;

        private PartyDataWrapper _partyData = null;
        
        public TownShopState(
            string shopName,
            IList<IItem> shopInventory,
            IList<IItem> partyInventory)
        {
            ShopName = shopName;
            _shopInventory = shopInventory;
            _partyInventory = partyInventory;
        }

        public override void OnEnter(IState fromState)
        {
            _partyData = SharedContext.Get<PartyDataWrapper>();

            // TODO: use party stats to mod these values plz
            _buyPriceMultiplier = 1f;
            _sellPriceMultiplier = 0.5f;
        }

        public override IEnumerable<IEventOption> GetOptions()
        {
            foreach (var item in _shopInventory)
            {
                uint price = (uint) (item.BaseValue * _buyPriceMultiplier);
                var cachedItem = item;
                yield return new EventOption(
                    $"{price}G",
                    () => BuyItem(cachedItem, price),
                    "Buy",
                    price,
                    price > _partyData.Gold,
                    cachedItem);
            }

            foreach (var item in _partyInventory)
            {
                uint price = (uint) (item.BaseValue * _sellPriceMultiplier);
                var cachedItem = item;
                yield return new EventOption(
                    $"{price}G",
                    () => SellItem(cachedItem, price),
                    "Sell",
                    price,
                    false,
                    cachedItem);
            }

            yield return new EventOption(
                "Leave",
                GoToTownHub);
        }

        private void BuyItem(IItem item, uint price)
        {
            if (item == null)
                return;
            if (!_shopInventory.Contains(item))
                return;
            if (price > _partyData.Gold)
                return;

            _shopInventory.Remove(item);
            _partyInventory.Add(item);
            _partyData.Inventory.Add(item);
            _partyData.Gold -= price;

            OptionsChangedSignal?.Fire(this);
        }

        private void SellItem(IItem item, uint price)
        {
            if (item == null)
                return;
            if (!_partyInventory.Contains(item))
                return;

            _partyInventory.Remove(item);
            _partyData.Inventory.Remove(item);
            _shopInventory.Add(item);
            _partyData.Gold += price;
            
            OptionsChangedSignal?.Fire(this);
        }

        private void GoToTownHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownHubState>();
        }
    }
}