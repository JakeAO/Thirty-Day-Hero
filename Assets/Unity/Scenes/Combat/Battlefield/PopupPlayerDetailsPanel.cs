using Core.Actors;
using Core.Actors.Player;
using Unity.Scenes.Shared.Status;
using UnityEngine;

namespace Unity.Scenes.Combat.Battlefield
{
    public class PopupPlayerDetailsPanel : MonoBehaviour
    {
        [SerializeField] private ClassLevelView _classLevelView;
        [SerializeField] private StatisticsView _statisticsView;
        [SerializeField] private EquipmentView _equipmentView;

        public void UpdateModel(ICharacterActor actorData)
        {
            _classLevelView.UpdateModel(actorData);
            _statisticsView.UpdateModel(actorData);
            _equipmentView.UpdateModel((actorData as IPlayerCharacterActor)?.Equipment);
        }
    }
}