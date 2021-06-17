using System;
using System.Collections.Generic;
using Core.Actors.Player;
using Core.Wrappers;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;

namespace Core.CharacterControllers
{
    public class PlayerCharacterController : ICharacterController
    {
        public uint PartyId { get; }
        public IReadOnlyCollection<IPlayerCharacterActor> Characters { get; }
        public IPlayerCharacterActor ActiveCharacter { get; private set; }
        public IReadOnlyDictionary<uint, IAction> AvailableActions { get; private set; }

        private Action<uint> _selectActionCallback = null;

        public PlayerCharacterController(PartyDataWrapper party)
        {
            PartyId = (uint) party.PartyId.GetHashCode();
            Characters = party.Characters;
        }

        public void SelectAction(IInitiativeActor activeEntity, IReadOnlyDictionary<uint, IAction> availableActions, Action<uint> selectAction)
        {
            ActiveCharacter = activeEntity as IPlayerCharacterActor;
            AvailableActions = availableActions;
            _selectActionCallback = selectAction;
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
        }
    }
}