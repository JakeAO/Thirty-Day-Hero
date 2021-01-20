using System.Collections.Generic;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Town
{
    public class TownInnState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";

        public readonly string InnName = null;

        public uint InnCost { get; private set; }

        public IReadOnlyDictionary<uint, (uint hp, uint sta)> RestoredStats { get; private set; }

        private PartyDataWrapper _partyData = null;
        private RestProvider _restProvider = null;

        public TownInnState(string shopName)
        {
            InnName = shopName;
        }

        public override void OnEnter(IState fromState)
        {
            _partyData = SharedContext.Get<PartyDataWrapper>();
            _restProvider = new RestProvider(1, 1);

            InnCost = 100;
        }

        public override void OnContent()
        {
            SetupOptions();
        }

        private void SetupOptions()
        {
            if (!_currentOptions.TryGetValue(CATEGORY_DEFAULT, out var defaultList))
                _currentOptions[CATEGORY_DEFAULT] = defaultList = new List<IEventOption>(2);

            defaultList.Clear();

            defaultList.Add(new EventOption(
                $"Rest ({InnCost}G)",
                OnRest,
                disabled: RestoredStats != null));
            defaultList.Add(new EventOption(
                "Leave",
                GoToTownHub));

            OptionsChangedSignal?.Fire(this);
        }

        private void OnRest()
        {
            _restProvider.RestParty(_partyData, out var restoredStats);

            RestoredStats = restoredStats;

            SetupOptions();
        }

        private void GoToTownHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownHubState>();
        }
    }
}