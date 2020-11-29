using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Items
{
    public interface IItem : IIdTracked, INamed
    {
        string ArtPath { get; }
        uint BaseValue { get; }
        RarityCategory Rarity { get; }
        ItemType ItemType { get; }
        IReadOnlyCollection<IAbility> AddedAbilities { get; }

        IReadOnlyCollection<IAction> GetAllActions(ICharacterActor sourceCharacter, IReadOnlyCollection<ITargetableActor> possibleTargets, bool isEquipped);

        // TODO Stat Bonus
    }
}