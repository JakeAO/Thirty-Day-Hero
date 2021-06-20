using Core.Abilities;
using TMPro;
using Unity.Scenes.Shared.Status;
using UnityEngine;

namespace Unity.Scenes.Combat.Results
{
    public class AbilityReward : MonoBehaviour, IAbilityView
    {
        [SerializeField] private TMP_Text _nameLabel;

        public void UpdateModel(IAbility ability)
        {
            _nameLabel.text = ability.Name;
        }
    }
}
