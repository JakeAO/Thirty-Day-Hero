using System;
using System.Collections.Generic;
using Core.Database;
using Core.EventOptions;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;
using Core.States.BaseClasses;
using Core.States.SubStates;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Town
{
    public class TownHubState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";

        private class TownDetails
        {
            public readonly List<IItem> ItemShopInventory = new List<IItem>(10);
            public readonly List<IItem> ArmorShopInventory = new List<IItem>(10);
            public readonly List<IItem> WeaponShopInventory = new List<IItem>(10);

            public bool HasItemShop = true;
            public bool HasArmorShop = true;
            public bool HasWeaponShop = true;
            public bool HasInn = true;
            public bool HasDojo = true;
        }

        private static Random RANDOM = new Random();

        private static uint _lastDayTimeTownEntered = UInt32.MaxValue;
        private static TownDetails _lastTownDetails = null;

        private PartyDataWrapper _partyData = null;

        public EquipmentSubState EquipmentSubState { get; private set; }

        public override void OnEnter(IState fromState)
        {
            _partyData = SharedContext.Get<PartyDataWrapper>();

            uint currentDayTime = _partyData.Day * 100 + (uint) _partyData.Time;

            if (_lastDayTimeTownEntered != currentDayTime ||
                _lastTownDetails == null)
            {
                _lastDayTimeTownEntered = currentDayTime;

                _lastTownDetails = new TownDetails
                {
                    HasItemShop = RANDOM.NextDouble() < 0.6f,
                    HasArmorShop = RANDOM.NextDouble() < 0.4f,
                    HasWeaponShop = RANDOM.NextDouble() < 0.4f,
                    HasInn = RANDOM.NextDouble() < 0.9f,
                    HasDojo = RANDOM.NextDouble() < 0.3f
                };

                if (_lastTownDetails.HasItemShop)
                {
                    uint itemsInInventory = (uint) Math.Round(5 + RANDOM.NextDouble() * 3);
                    _lastTownDetails.ItemShopInventory.AddRange(SharedContext.Get<ItemDatabase>().GetRandom(itemsInInventory));
                }

                if (_lastTownDetails.HasArmorShop)
                {
                    uint armorInInventory = (uint) Math.Round(2 + RANDOM.NextDouble() * 3);
                    _lastTownDetails.ArmorShopInventory.AddRange(SharedContext.Get<ArmorDatabase>().GetRandom(armorInInventory));
                }

                if (_lastTownDetails.HasWeaponShop)
                {
                    uint weaponsInInventory = (uint) Math.Round(2 + RANDOM.NextDouble() * 3);
                    _lastTownDetails.WeaponShopInventory.AddRange(SharedContext.Get<WeaponDatabase>().GetRandom(weaponsInInventory));
                }
            }

            EquipmentSubState = new EquipmentSubState(SharedContext);
        }

        public override void OnContent()
        {
            SetupOptions();
        }

        private void SetupOptions()
        {
            _currentOptions.Clear();

            if (EquipmentSubState.Active)
            {
                SetupOptions_ChangeEquipment();
            }
            else
            {
                SetupOptions_Default();
            }
        }

        private void SetupOptions_Default()
        {
            var defaultList = _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>(5);
            
            defaultList.Add(new EventOption(
                "Change Equipment",
                OpenEquipment,
                CATEGORY_DEFAULT));

            if (_lastTownDetails.HasItemShop)
            {
                defaultList.Add(new EventOption(
                    "Enter Item Shop",
                    GoToItemShop,
                    CATEGORY_DEFAULT));
            }

            if (_lastTownDetails.HasArmorShop)
            {
                defaultList.Add(new EventOption(
                    "Enter Armor Shop",
                    GoToArmorShop,
                    CATEGORY_DEFAULT));
            }

            if (_lastTownDetails.HasWeaponShop)
            {
                defaultList.Add(new EventOption(
                    "Enter Weapon Shop",
                    GoToWeaponShop,
                    CATEGORY_DEFAULT));
            }

            if (_lastTownDetails.HasDojo)
            {
                defaultList.Add(new EventOption(
                    "Enter Dojo",
                    GoToDojo,
                    CATEGORY_DEFAULT));
            }

            if (_lastTownDetails.HasInn)
            {
                defaultList.Add(new EventOption(
                    "Enter Inn",
                    GoToInn,
                    CATEGORY_DEFAULT));
            }

            defaultList.Add(new EventOption(
                "Leave Town",
                GoToGameHub,
                CATEGORY_DEFAULT));
        }

        private void SetupOptions_ChangeEquipment()
        {
            foreach (var optionsKvp in EquipmentSubState.CurrentOptions)
            {
                _currentOptions[optionsKvp.Key] = optionsKvp.Value;
            }

            var defaultList = _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>(5);
            defaultList.Add(new EventOption(
                "Stop Equipping",
                CloseEquipment,
                CATEGORY_DEFAULT));
        }

        private void OpenEquipment()
        {
            EquipmentSubState.Active = true;
            SetupOptions();
        }

        private void CloseEquipment()
        {
            EquipmentSubState.Active = false;
            SetupOptions();
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }

        private void GoToItemShop()
        {
            SharedContext.Get<IStateMachine>().ChangeState(
                new TownShopState("Item Shop",
                    _lastTownDetails.ItemShopInventory,
                    _partyData.Inventory.FindAll(x => x is Item)));
        }

        private void GoToArmorShop()
        {
            SharedContext.Get<IStateMachine>().ChangeState(
                new TownShopState("Armor Shop",
                    _lastTownDetails.ArmorShopInventory,
                    _partyData.Inventory.FindAll(x => x is Armor)));
        }

        private void GoToWeaponShop()
        {
            SharedContext.Get<IStateMachine>().ChangeState(
                new TownShopState("Weapon Shop",
                    _lastTownDetails.WeaponShopInventory,
                    _partyData.Inventory.FindAll(x => x is Weapon)));
        }

        private void GoToDojo()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownDojoState>();
        }

        private void GoToInn()
        {
            SharedContext.Get<IStateMachine>().ChangeState(
                new TownInnState("Inn"));
        }
    }
}