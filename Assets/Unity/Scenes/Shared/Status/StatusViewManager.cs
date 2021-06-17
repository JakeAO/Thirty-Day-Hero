using System.Collections.Generic;
using Core.Actors;
using Unity.Scenes.Shared.Pooling;

namespace Unity.Scenes.Shared.Status
{
    public class StatusViewManager : IStatusViewManager
    {
        private readonly IUnityPool _unityPool = null;
        private readonly StatusView _statusViewPrefab = null;
        private readonly HealthView _healthViewPrefab = null;
        private readonly StaminaView _staminaViewPrefab = null;
        private readonly StatusEffectView _statusEffectViewPrefab = null;

        private readonly Dictionary<uint, IStatusView> _statusViews = new Dictionary<uint, IStatusView>(10);
        private readonly Dictionary<uint, IHealthView> _healthViews = new Dictionary<uint, IHealthView>(10);
        private readonly Dictionary<uint, IStaminaView> _staminaViews = new Dictionary<uint, IStaminaView>(10);
        private readonly Dictionary<uint, IStatusEffectView> _statusEffectViews = new Dictionary<uint, IStatusEffectView>(10);

        public StatusViewManager(
            IUnityPool unityPool,
            StatusView statusViewPrefab,
            HealthView healthViewPrefab,
            StaminaView staminaViewPrefab,
            StatusEffectView statusEffectViewPrefab)
        {
            _unityPool = unityPool;
            _statusViewPrefab = statusViewPrefab;
            _healthViewPrefab = healthViewPrefab;
            _staminaViewPrefab = staminaViewPrefab;
            _statusEffectViewPrefab = statusEffectViewPrefab;
        }

        public IStatusView CreateStatusView(ICharacterActor actorData)
        {
            if (_statusViews.TryGetValue(actorData.Id, out IStatusView existingView))
            {
                existingView.UpdateModel(actorData);
                return existingView;
            }

            IStatusView newView = _unityPool.Pop(_statusViewPrefab, null);
            newView.UpdateModel(actorData);
            return newView;
        }

        public IHealthView CreateHealthView(ICharacterActor actorData)
        {
            if (_healthViews.TryGetValue(actorData.Id, out IHealthView existingView))
            {
                existingView.UpdateModel(actorData);
                return existingView;
            }

            IHealthView newView = _unityPool.Pop(_healthViewPrefab, null);
            newView.UpdateModel(actorData);
            return newView;
        }

        public IStaminaView CreateStaminaView(ICharacterActor actorData)
        {
            if (_staminaViews.TryGetValue(actorData.Id, out IStaminaView existingView))
            {
                existingView.UpdateModel(actorData);
                return existingView;
            }

            IStaminaView newView = _unityPool.Pop(_staminaViewPrefab, null);
            newView.UpdateModel(actorData);
            return newView;
        }

        public IStatusEffectView CreateStatusEffectView(ICharacterActor actorData)
        {
            if (_statusEffectViews.TryGetValue(actorData.Id, out IStatusEffectView existingView))
            {
                existingView.UpdateModel(actorData);
                return existingView;
            }

            IStatusEffectView newView = _unityPool.Pop(_statusEffectViewPrefab, null);
            newView.UpdateModel(actorData);
            return newView;
        }

        public IStatusView GetStatusView(uint actorId) =>
            _statusViews.TryGetValue(actorId, out IStatusView existingView)
                ? existingView
                : null;

        public IHealthView GetHealthView(uint actorId) =>
            _healthViews.TryGetValue(actorId, out IHealthView existingView)
                ? existingView
                : _statusViews.TryGetValue(actorId, out IStatusView parentView)
                    ? parentView.HealthView
                    : null;

        public IStaminaView GetStaminaView(uint actorId) =>
            _staminaViews.TryGetValue(actorId, out IStaminaView existingView)
                ? existingView
                : _statusViews.TryGetValue(actorId, out IStatusView parentView)
                    ? parentView.StaminaView
                    : null;

        public IStatusEffectView GetStatusEffectView(uint actorId) =>
            _statusEffectViews.TryGetValue(actorId, out IStatusEffectView existingView)
                ? existingView
                : _statusViews.TryGetValue(actorId, out IStatusView parentView)
                    ? parentView.StatusEffectView
                    : null;

        public void UpdateStatusView(ICharacterActor actorData)
        {
            if (_statusViews.TryGetValue(actorData.Id, out IStatusView existingView))
            {
                existingView.UpdateModel(actorData);
            }
        }

        public void UpdateHealthView(ICharacterActor actorData)
        {
            if (_healthViews.TryGetValue(actorData.Id, out IHealthView existingView))
            {
                existingView.UpdateModel(actorData);
            }
            else if (_statusViews.TryGetValue(actorData.Id, out IStatusView parentView))
            {
                parentView.HealthView.UpdateModel(actorData);
            }
        }

        public void UpdateStaminaView(ICharacterActor actorData)
        {
            if (_staminaViews.TryGetValue(actorData.Id, out IStaminaView existingView))
            {
                existingView.UpdateModel(actorData);
            }
            else if (_statusViews.TryGetValue(actorData.Id, out IStatusView parentView))
            {
                parentView.StaminaView.UpdateModel(actorData);
            }
        }

        public void UpdateStatusEffectView(ICharacterActor actorData)
        {
            if (_statusEffectViews.TryGetValue(actorData.Id, out IStatusEffectView existingView))
            {
                existingView.UpdateModel(actorData);
            }
            else if (_statusViews.TryGetValue(actorData.Id, out IStatusView parentView))
            {
                parentView.StatusEffectView.UpdateModel(actorData);
            }
        }

        public void DeleteStatusView(uint actorId)
        {
            if (_statusViews.TryGetValue(actorId, out IStatusView existingView))
            {
                _statusViews.Remove(actorId);

                if (existingView is StatusView concreteView)
                {
                    _unityPool.Push(concreteView);
                }
            }
        }

        public void DeleteHealthView(uint actorId)
        {
            if (_healthViews.TryGetValue(actorId, out IHealthView existingView))
            {
                _healthViews.Remove(actorId);

                if (existingView is HealthView concreteView)
                {
                    _unityPool.Push(concreteView);
                }
            }
        }

        public void DeleteStaminaView(uint actorId)
        {
            if (_staminaViews.TryGetValue(actorId, out IStaminaView existingView))
            {
                _staminaViews.Remove(actorId);

                if (existingView is StaminaView concreteView)
                {
                    _unityPool.Push(concreteView);
                }
            }
        }

        public void DeleteStatusEffectView(uint actorId)
        {
            if (_statusEffectViews.TryGetValue(actorId, out IStatusEffectView existingView))
            {
                _statusEffectViews.Remove(actorId);

                if (existingView is StatusEffectView concreteView)
                {
                    _unityPool.Push(concreteView);
                }
            }
        }
    }
}