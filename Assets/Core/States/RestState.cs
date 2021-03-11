using System.Collections.Generic;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.States.SubStates;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class RestState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";

        public IReadOnlyDictionary<uint, (uint hp, uint sta)> RestoredStats { get; private set; }

        private PartyDataWrapper _partyData;

        private RestProvider _restProvider = null;

        public EquipmentSubState EquipmentSubState { get; private set; }

        public override void OnEnter(IState fromState)
        {
            _partyData = SharedContext.Get<PartyDataWrapper>();
            _restProvider = _partyData.Time == TimeOfDay.Night
                ? new RestProvider(0.5, 0.5)
                : new RestProvider(0.2, 0.2);

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

            OptionsChangedSignal?.Fire(this);
        }

        private void SetupOptions_Default()
        {
            var defaultList = _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>(5);
            defaultList.Add(new EventOption(
                "Change Equipment",
                OpenEquipment,
                CATEGORY_DEFAULT));
            defaultList.Add(new EventOption(
                "Rest",
                OnRest,
                CATEGORY_DEFAULT,
                disabled: RestoredStats != null));
            defaultList.Add(new EventOption(
                "Talk",
                OnTalk,
                CATEGORY_DEFAULT,
                disabled: _partyData.Characters.Count == 1));
            defaultList.Add(new EventOption(
                "Leave",
                GoToGameHub,
                CATEGORY_DEFAULT));
        }

        private void SetupOptions_ChangeEquipment()
        {
            var defaultList = _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>(5);
            foreach (var optionsKvp in EquipmentSubState.CurrentOptions)
            {
                _currentOptions[optionsKvp.Key] = optionsKvp.Value;
            }

            defaultList.Clear();
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

        private void OnRest()
        {
            _restProvider.RestParty(_partyData, out var restoredStats);

            RestoredStats = restoredStats;

            SetupOptions();
        }

        private void OnTalk()
        {
            // TODO
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }
    }
}