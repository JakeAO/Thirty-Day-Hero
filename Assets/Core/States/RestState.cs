using System.Collections.Generic;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
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

        public override void OnEnter(IState fromState)
        {
            _partyData = SharedContext.Get<PartyDataWrapper>();
            _restProvider = _partyData.Time == TimeOfDay.Night
                ? new RestProvider(0.5, 0.5)
                : new RestProvider(0.2, 0.2);

        }

        public override void OnContent()
        {
            SetupOptions();
        }

        private void SetupOptions()
        {
            if (!_currentOptions.TryGetValue(CATEGORY_DEFAULT, out var defaultList))
                _currentOptions[CATEGORY_DEFAULT] = defaultList = new List<IEventOption>(3);

            defaultList.Clear();

            defaultList.Add(new EventOption(
                "Rest",
                OnRest,
                disabled: RestoredStats != null));
            defaultList.Add(new EventOption(
                "Talk",
                OnTalk,
                disabled: _partyData.Characters.Count == 1));
            defaultList.Add(new EventOption(
                "Leave",
                GoToGameHub));

            OptionsChangedSignal?.Fire(this);
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