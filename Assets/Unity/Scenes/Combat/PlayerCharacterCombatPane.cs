using Core.Actors.Player;
using Core.Etc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scenes.Combat
{
    public class PlayerCharacterCombatPane : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private TMP_Text _hpLabel;
        [SerializeField] private Slider _staSlider;
        [SerializeField] private TMP_Text _staLabel;
        [SerializeField] private GridLayoutGroup _indicatorGrid;

        public void UpdateCharacter(IPlayerCharacterActor character)
        {
            // Name
            _nameLabel.text = character.Name;
            _nameLabel.color = character.IsAlive() ? Color.black : Color.red;

            // HP
            _hpSlider.minValue = 0u;
            _hpSlider.maxValue = character.Stats[StatType.HP_Max];
            _hpSlider.value = character.Stats[StatType.HP];
            _hpLabel.text = $"{character.Stats[StatType.HP]}/{character.Stats[StatType.HP_Max]}";

            // STA
            _staSlider.minValue = 0u;
            _staSlider.maxValue = character.Stats[StatType.STA_Max];
            _staSlider.value = character.Stats[StatType.STA];
            _staLabel.text = $"{character.Stats[StatType.STA]}/{character.Stats[StatType.STA_Max]}";

            // Indicators
            foreach (Transform indicator in _indicatorGrid.transform)
            {
                Destroy(indicator.gameObject);
            }
        }
    }
}