using System;
using System.Collections.Generic;
using Core.Actors.Player;
using Core.Signals;
using Core.Wrappers;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;
using SadPumpkin.Util.CombatEngine.Signals;
using SadPumpkin.Util.Signals;

namespace Core.CharacterControllers
{
    public class PlayerCharacterController : ICharacterController
    {
        public uint PartyId { get; }
        public GameStateUpdated GameStateUpdatedSignal { get; }
        public CombatComplete CombatCompleteSignal { get; }
        public IReadOnlyCollection<IPlayerCharacterActor> Characters { get; }
        public IPlayerCharacterActor ActiveCharacter { get; private set; }
        public IReadOnlyDictionary<uint, IAction> AvailableActions { get; private set; }

        public ISignal<IInitiativeActor> ActiveCharacterChanged { get; }

        private Action<uint> _selectActionCallback = null;

        public PlayerCharacterController(
            PartyDataWrapper party,
            ISignal<IInitiativeActor> activeEntityChangedSignal)
        {
            ActiveCharacterChanged = activeEntityChangedSignal ?? new ActiveCharacterChangedSignal();

            PartyId = (uint) party.PartyId.GetHashCode();
            Characters = party.Characters;

            GameStateUpdatedSignal = new GameStateUpdated();
            CombatCompleteSignal = new CombatComplete();
        }

        public void SelectAction(IInitiativeActor activeEntity, IReadOnlyDictionary<uint, IAction> availableActions, Action<uint> selectAction)
        {
            ActiveCharacter = activeEntity as IPlayerCharacterActor;
            AvailableActions = availableActions;
            _selectActionCallback = selectAction;

            ActiveCharacterChanged.Fire(activeEntity);
        }

        public void SubmitActionResponse(uint actionId)
        {
            if (ActiveCharacter == null ||
                AvailableActions == null ||
                !AvailableActions.ContainsKey(actionId))
            {
                return;
            }

            _selectActionCallback?.Invoke(actionId);

            ActiveCharacter = null;
            AvailableActions = null;
            _selectActionCallback = null;

            ActiveCharacterChanged.Fire(null);
        }
    }
}