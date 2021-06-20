using Core.Actors;
using Core.Etc;
using TMPro;
using Unity.Utility;
using UnityEngine;

namespace Unity.Scenes.Shared.Status
{
    public class HealthView : MonoBehaviour, IHealthView
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private SlicedFilledImage _slider;

        public void UpdateModel(ICharacterActor actorData)
        {
            uint current = actorData.Stats[StatType.HP];
            uint max = actorData.Stats[StatType.HP_Max];

            _slider.fillAmount = current / (float) max;
            _label.text = $"{current}/{max}";
        }
    }
}