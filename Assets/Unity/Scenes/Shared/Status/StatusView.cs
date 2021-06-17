using Core.Actors;
using UnityEngine;

namespace Unity.Scenes.Shared.Status
{
    public class StatusView : MonoBehaviour, IStatusView
    {
        [SerializeField] private HealthView _healthView = null;
        [SerializeField] private StaminaView _staminaView = null;
        [SerializeField] private StatusEffectView _statusEffectView = null;

        public IHealthView HealthView => _healthView;
        public IStaminaView StaminaView => _staminaView;
        public IStatusEffectView StatusEffectView => _statusEffectView;

        public void UpdateModel(ICharacterActor actorData)
        {
            _healthView.UpdateModel(actorData);
            _staminaView.UpdateModel(actorData);
            _statusEffectView.UpdateModel(actorData);
        }
    }
}