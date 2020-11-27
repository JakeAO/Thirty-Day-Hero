using System.Collections.Generic;
using Core.Actors.Enemy;
using SadPumpkin.Util.CombatEngine.CharacterControllers;

namespace Core.CombatSettings
{
    public class CombatSettings
    {
        public IReadOnlyCollection<IEnemyCharacterActor> Enemies { get; }
        public ICharacterController Controller { get; }

        public CombatSettings(IReadOnlyCollection<IEnemyCharacterActor> enemies, ICharacterController controller)
        {
            Enemies = enemies;
            Controller = controller;
        }
    }
}