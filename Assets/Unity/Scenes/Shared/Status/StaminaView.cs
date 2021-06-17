using Core.Actors;
using Core.Etc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scenes.Shared.Status
{
    public class StaminaView : MonoBehaviour, IStaminaView
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Slider _slider;

        public void UpdateModel(ICharacterActor actorData)
        {
            uint current = actorData.Stats[StatType.STA];
            uint max = actorData.Stats[StatType.STA_Max];

            _slider.minValue = 0f;
            _slider.maxValue = max;
            _slider.value = current;

            _label.text = $"{current}/{max}";
        }
    }
}