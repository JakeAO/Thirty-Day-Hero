using System.Collections.Generic;
using Core.Etc;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Items
{
    public interface IItem : IIdTracked
    {
        string Name { get; }
        string Desc { get; }
        ItemType ItemType { get; }
        IReadOnlyCollection<IAbility> AddedAbilities { get; }

        IReadOnlyCollection<IAction> GetAllActions(ICharacterActor sourceCharacter, IReadOnlyCollection<ITargetableActor> possibleTargets, bool isEquipped);

        // TODO Stat Bonus
    }
}