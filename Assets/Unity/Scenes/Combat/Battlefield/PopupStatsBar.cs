using Core.Actors;
using Core.Etc;
using Unity.Scenes.Shared.Status;
using Unity.Utility;
using UnityEngine;

namespace Unity.Scenes.Combat.Battlefield
{
    public class PopupStatsBar : MonoBehaviour, IHealthView, IStaminaView
    {
        [SerializeField] private SlicedFilledImage _hpFill;
        [SerializeField] private SlicedFilledImage _staFill;

        public void UpdateModel(ICharacterActor actorData)
        {
            _hpFill.fillAmount = actorData.Stats[StatType.HP] / (float) actorData.Stats[StatType.HP_Max];
            _staFill.fillAmount = actorData.Stats[StatType.STA] / (float) actorData.Stats[StatType.STA_Max];
        }
    }
}
