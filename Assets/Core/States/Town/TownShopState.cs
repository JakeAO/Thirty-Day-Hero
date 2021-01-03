using System.Collections.Generic;
using Core.EventOptions;
using Core.Items;
using Core.States.BaseClasses;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Town
{
    public class TownShopState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";
        public const string CATEGORY_BUY = "Buy";
        public const string CATEGORY_SELL = "Sell";
        
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

        public override void OnContent()
        {
            SetupOptions();
        }

        private void SetupOptions()
        {
            // Get/Create option lists
            if (!_currentOptions.TryGetValue(CATEGORY_DEFAULT, out var defaultList))
                _currentOptions[CATEGORY_DEFAULT] = defaultList = new List<IEventOption>(1);
            if (!_currentOptions.TryGetValue(CATEGORY_BUY, out var buyList))
                _currentOptions[CATEGORY_BUY] = buyList = new List<IEventOption>(10);
            if (!_currentOptions.TryGetValue(CATEGORY_SELL, out var sellList))
                _currentOptions[CATEGORY_SELL] = sellList = new List<IEventOption>(10);

            // Clear lists
            defaultList.Clear();
            buyList.Clear();
            sellList.Clear();
            
            // Add items from the shop's inventory
            foreach (var item in _shopInventory)
            {
                uint price = (uint) (item.BaseValue * _buyPriceMultiplier);
                var cachedItem = item;
                buyList.Add(new EventOption(
                    $"{price}G",
                    () => BuyItem(cachedItem, price),
                    CATEGORY_BUY,
                    price,
                    price > _partyData.Gold,
                    cachedItem));
            }

            // Add items from the player's inventory
            foreach (var item in _partyInventory)
            {
                uint price = (uint) (item.BaseValue * _sellPriceMultiplier);
                var cachedItem = item;
                sellList.Add(new EventOption(
                    $"{price}G",
                    () => SellItem(cachedItem, price),
                    CATEGORY_SELL,
                    price,
                    false,
                    cachedItem));
            }

            // Ensure the player can leave
            defaultList.Add(new EventOption(
                "Leave",
                GoToTownHub));

            OptionsChangedSignal?.Fire(this);
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
            
            SetupOptions();
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
            
            SetupOptions();
        }

        private void GoToTownHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownHubState>();
        }
    }
}