using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;
using SadPumpkin.Util.CombatEngine.Party;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Party
{
    public class Party : IParty
    {
        public uint Id { get; }
        public ICharacterController Controller { get; }
        public IReadOnlyCollection<IInitiativeActor> Actors { get; }

        public Party(
            uint id,
            ICharacterController controller,
            IReadOnlyCollection<ICharacterActor> characters)
        {
            Id = id;
            Controller = controller;
            Actors = new List<IInitiativeActor>(characters);
        }
    }
}