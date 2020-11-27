using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

using ActionUtil = Core.Actions.ActionUtil;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Items
{
    public class Item : IItem
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ArtPath { get; set; }
        public uint BaseValue { get; set; }
        public RarityCategory Rarity { get; set; }
        public ItemType ItemType { get; set; }
        public IReadOnlyCollection<IAbility> AddedAbilities { get; set; }

        public Item()
            : this(0,
                string.Empty,
                string.Empty,
                string.Empty,
                0u,
                RarityCategory.Invalid,
                ItemType.Invalid,
                null)
        {
        }

        public Item(
            uint id,
            string name,
            string desc,
            string artPath,
            uint baseValue,
            RarityCategory rarity,
            ItemType itemType,
            IReadOnlyCollection<IAbility> addedAbilities)
        {
            Id = id;
            Name = name;
            Desc = desc;
            ArtPath = artPath;
            BaseValue = baseValue;
            Rarity = rarity;
            ItemType = itemType;
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