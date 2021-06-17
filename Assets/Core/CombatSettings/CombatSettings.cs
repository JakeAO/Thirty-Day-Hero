using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Enemy;
using SadPumpkin.Util.CombatEngine.CharacterControllers;
using SadPumpkin.Util.CombatEngine.Party;

namespace Core.CombatSettings
{
    public class CombatSettings
    {
        public uint EnemyPartyId { get; }
        public IReadOnlyCollection<ICharacterActor> Enemies { get; }
        public ICharacterController Controller { get; }

        public CombatSettings(uint enemyPartyId, IReadOnlyCollection<ICharacterActor> enemies, ICharacterController controller)
        {
            EnemyPartyId = enemyPartyId;
            Enemies = enemies;
            Controller = controller;
        }

        public IParty GetAsParty() => new Party.Party(
            EnemyPartyId,
            Controller,
            Enemies);
    }
}