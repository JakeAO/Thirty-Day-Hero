using System;
using Core.Actors;
using TMPro;
using Unity.Extensions;
using Unity.Scenes.Combat.Etc;
using Unity.Scenes.Shared.Entities;
using Unity.Scenes.Shared.Status;
using Unity.Scenes.Shared.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes.Combat.Battlefield
{
    [RequireComponent(typeof(UnityInteractionHandler))]
    public class CombatPlayerView : MonoBehaviour, IActorView, IHighlightableView, IInteractableView<IActorView>
    {
        [SerializeField] private Image _classIcon;
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private HealthView _hpView;
        [SerializeField] private StaminaView _staView;
        [SerializeField] private StatusEffectView _statusEffectView;
        [SerializeField] private PopupPlayerDetailsPanel _detailsPanel;
        [SerializeField] private TargetingIndicator _targetingIndicator;

        public event Action<IActorView, InteractionType> Interacted;

        public ICharacterActor Model { get; private set; }

        private void Awake()
        {
            GetComponent<UnityInteractionHandler>().Interacted += OnInteracted;
        }

        public void InitializeModel(ICharacterActor actorData)
        {
            Model = actorData;

            _nameLabel.text = actorData.Name;
            _hpView.UpdateModel(actorData);
            _staView.UpdateModel(actorData);
            _statusEffectView.UpdateModel(actorData);
            _detailsPanel.UpdateModel(actorData);
            _targetingIndicator.SetHighlight(HighlightType.None);

            Addressables.LoadAssetAsync<Sprite>(actorData.Class.ArtPath).Completed += OnSpriteLoaded;
        }

        public void UpdateModel(ICharacterActor actorData, ActorUpdateContext context)
        {
            Model = actorData;

            _nameLabel.text = actorData.Name;
            _hpView.UpdateModel(actorData);
            _staView.UpdateModel(actorData);
            _statusEffectView.UpdateModel(actorData);
            _detailsPanel.UpdateModel(actorData);
        }

        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
        {
            _classIcon.sprite = handle.Result;
        }

        public void SetHighlight(HighlightType highlightType)
        {
            _targetingIndicator.SetHighlight(highlightType);
        }

        private void OnInteracted(InteractionType interactionType)
        {
            // Show player stats on hover
            _detailsPanel.gameObject.UpdateActive(interactionType == InteractionType.Hover);
            
            Interacted?.Invoke(this, interactionType);
        }
    }
}