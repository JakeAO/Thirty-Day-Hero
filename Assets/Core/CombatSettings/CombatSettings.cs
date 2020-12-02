using System.Collections.Generic;
using Core.Actors.Enemy;
using SadPumpkin.Util.CombatEngine.CharacterControllers;

namespace Core.CombatSettings
{
    public class CombatSettings
    {
        public uint EnemyPartyId { get; }
        public IReadOnlyCollection<IEnemyCharacterActor> Enemies { get; }
        public ICharacterController Controller { get; }

        public CombatSettings(uint enemyPartyId, IReadOnlyCollection<IEnemyCharacterActor> enemies, ICharacterController controller)
        {
            EnemyPartyId = enemyPartyId;
            Enemies = enemies;
            Controller = controller;
        }
    }
}