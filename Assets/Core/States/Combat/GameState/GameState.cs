using System.Collections.Generic;
using Core.Actors;

namespace Core.States.Combat.GameState
{
    public class GameState : IGameState
    {
        private readonly Dictionary<uint, ICharacterActor> _actors = new Dictionary<uint, ICharacterActor>(10);
        private ICharacterActor _activeCharacter;
        
        public GameState(
            IEnumerable<ICharacterActor> playerActors,
            IEnumerable<ICharacterActor> enemyActors)
        {
            foreach (ICharacterActor actor in playerActors)
            {
                _actors[actor.Id] = actor;
            }

            foreach (ICharacterActor actor in enemyActors)
            {
                _actors[actor.Id] = actor;
            }
        }

        public void SetActiveActor(ICharacterActor actor)
        {
            _activeCharacter = actor;
        }

        public bool TryGetActor(uint actorId, out ICharacterActor actor) => _actors.TryGetValue(actorId, out actor);
        public ICharacterActor GetActor(uint actorId) => TryGetActor(actorId, out ICharacterActor actor) ? actor : null;
        public IEnumerable<ICharacterActor> AllActors => _actors.Values;

        public ICharacterActor ActiveActor => _activeCharacter;
    }
}