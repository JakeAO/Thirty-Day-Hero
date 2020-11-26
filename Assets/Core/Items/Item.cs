using System;
using System.Collections.Generic;
using Core.Etc;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

using ActionUtil = Core.Actions.ActionUtil;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Items
{
    public class Item : IItem
    {
        public uint Id { get; }
        public string Name { get; }
        public string Desc { get; }
        public ItemType ItemType { get; }
        public IReadOnlyCollection<IAbility> AddedAbilities { get; }

        public Item()
            : this(0,
                ItemType.Invalid,
                string.Empty, String.Empty,
                null)
        {
        }

        public Item(
            uint id,
            ItemType itemType,
            string name, string desc,
            IReadOnlyCollection<IAbility> addedAbilities)
        {
            Id = id;
            ItemType = itemType;
            Name = name;
            Desc = desc;
            AddedAbilities = addedAbilities != null
                ? new List<IAbility>(addedAbilities)
                : new List<IAbility>();
        }

        public IReadOnlyCollection<IAction> GetAllActions(ICharacterActor sourceCharacter,
            IReadOnlyCollection<ITargetableActor> possibleTargets, bool isEquipped)
        {
            List<IAction> actions = new List<IAction>(10);

            if (AddedAbilities != null)
            {
                foreach (IAbility ability in AddedAbilities)
                {
                    actions.AddRange(
                        ActionUtil.GetActionsFromAbility(
                            sourceCharacter,
                            this,
                            ability,
                            possibleTargets));
                }
            }

            return actions;
        }
    }
}