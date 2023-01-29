using Core.Abilities;
using Core.Actors.Player;
using Core.Etc;
using TMPro;
using Unity.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes.Combat.Results
{
    public class StatReward : MonoBehaviour
    {
        [SerializeField] private Image _classIcon;
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private TMP_Text _levelLabel;
        [SerializeField] private StatChangeReward _hpReward;
        [SerializeField] private StatChangeReward _staReward;
        [SerializeField] private StatChangeReward _strReward;
        [SerializeField] private StatChangeReward _dexReward;
        [SerializeField] private StatChangeReward _conReward;
        [SerializeField] private StatChangeReward _intReward;
        [SerializeField] private StatChangeReward _magReward;
        [SerializeField] private StatChangeReward _chaReward;
        [SerializeField] private RectTransform _abilityRewardRoot;
        [SerializeField] private AbilityReward _abilityRewardPrefab;

        public void Show(IPlayerCharacterActor statChangeActor, int[] statChanges)
        {
            if (!string.IsNullOrWhiteSpace(statChangeActor.Class.ArtPath))
            {
                Addressables.LoadAssetAsync<Sprite>(statChangeActor.Class.ArtPath).Completed += OnSpriteLoaded;
            }
            else
            {
                _classIcon.sprite = SpriteExtensions.RedSprite;
            }

            _nameLabel.text = statChangeActor.Name;
            _levelLabel.text = $"+{statChanges[(int) StatType.LVL]}";

            _hpReward.SetValue(statChanges[(int) StatType.HP_Max]);
            _staReward.SetValue(statChanges[(int) StatType.STA_Max]);
            _strReward.SetValue(statChanges[(int) StatType.STR]);
            _dexReward.SetValue(statChanges[(int) StatType.DEX]);
            _conReward.SetValue(statChanges[(int) StatType.CON]);
            _intReward.SetValue(statChanges[(int) StatType.INT]);
            _magReward.SetValue(statChanges[(int) StatType.MAG]);
            _chaReward.SetValue(statChanges[(int) StatType.CHA]);

            uint currentLevel = statChangeActor.Stats[StatType.LVL];
            uint prevLevel = (uint) (currentLevel - statChanges[(int) StatType.LVL]);
            for (uint lvl = prevLevel + 1; lvl <= currentLevel; lvl++)
            {
                if (statChangeActor.Class.AbilitiesPerLevel.TryGetValue(lvl, out var abilitiesForLvl))
                {
                    foreach (IAbility learnedAbility in abilitiesForLvl)
                    {
                        AbilityReward abilityView = Instantiate(_abilityRewardPrefab, _abilityRewardRoot);
                        abilityView.UpdateModel(learnedAbility);
                    }
                }
            }
            _abilityRewardRoot.UpdateActive(_abilityRewardRoot.childCount > 0);
        }

        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
        {
            _classIcon.sprite = handle.Result;
        }
    }
}
