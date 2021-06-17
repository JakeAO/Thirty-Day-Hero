using System.Collections.Generic;
using Core.Actors;

namespace Core.States.Combat
{
    public class CombatDataWrapper
    {
        private readonly Dictionary<uint, ICharacterActor> _actors = new Dictionary<uint, ICharacterActor>(10);

        public CombatDataWrapper(
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

        public bool TryGetActor(uint actorId, out ICharacterActor actor) => _actors.TryGetValue(actorId, out actor);
        public ICharacterActor GetActor(uint actorId) => TryGetActor(actorId, out ICharacterActor actor) ? actor : null;
        public IEnumerable<ICharacterActor> AllActors => _actors.Values;
    }
}