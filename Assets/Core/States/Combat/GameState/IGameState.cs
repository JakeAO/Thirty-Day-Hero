using System.Collections.Generic;
using Core.Actors;

namespace Core.States.Combat.GameState
{
    public interface IGameState
    {
        // Actor(s)
        bool TryGetActor(uint actorId, out ICharacterActor actor);
        ICharacterActor GetActor(uint actorId);
        IEnumerable<ICharacterActor> AllActors { get; }
        
        // Active Actor
        ICharacterActor ActiveActor { get; }
    }
}