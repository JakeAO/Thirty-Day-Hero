﻿using Core.Actors;
using Core.Actors.Enemy;
using Core.Classes.Enemy;
using Core.Party;
using Core.StatMap;
using NUnit.Framework;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.CharacterControllers;

[TestFixture]
public class CombatManagerTests
{
    [Test]
    public void combat_manager_can_be_created()
    {
        CombatManager combatManager = new CombatManager(
            new[]
            {
                new Party(1,
                    new RandomCharacterController(),
                    new ICharacterActor[]
                    {
                        new EnemyCharacter(10, 1, "Geoff", NullEnemyClass.Instance, NullStatMap.Instance),
                    }),
                new Party(2,
                    new RandomCharacterController(),
                    new ICharacterActor[]
                    {
                        new EnemyCharacter(11, 2, "Jeff", NullEnemyClass.Instance, NullStatMap.Instance),
                    }),
            },
            new NullStandardActionGenerator(),
            null,
            null);

        Assert.IsNotNull(combatManager);
        Assert.IsInstanceOf<CombatManager>(combatManager);
    }
}
