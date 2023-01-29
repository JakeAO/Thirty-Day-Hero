using System;
using Core.Actors;
using Unity.Extensions;
using Unity.Scenes.Combat.Etc;
using Unity.Scenes.Shared.Entities;
using Unity.Scenes.Shared.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes.Combat.Battlefield
{
    [RequireComponent(typeof(UnityInteractionHandler))]
    public class CombatEnemyView : MonoBehaviour, IActorView, IInteractableView<IActorView>, IHighlightableView
    {
        [SerializeField] private Image _image;
        [SerializeField] private PopupStatsBar _statsBar;
        [SerializeField] private TargetingIndicator _targetingRing;

        public event Action<IActorView, InteractionType> Interacted;

        public ICharacterActor Model { get; private set; }

        private void Awake()
        {
            GetComponent<UnityInteractionHandler>().Interacted += OnInteraction;
        }

        public void InitializeModel(ICharacterActor actorData)
        {
            Model = actorData;

            _statsBar.UpdateModel(actorData);
            _targetingRing.SetHighlight(HighlightType.None);

            if (!string.IsNullOrWhiteSpace(actorData.Class.ArtPath))
            {
                Addressables.LoadAssetAsync<Sprite>(actorData.Class.ArtPath).Completed += OnSpriteLoaded;
            }
            else
            {
                _image.sprite = SpriteExtensions.RedSprite;
            }
        }

        public void UpdateModel(ICharacterActor actorData, ActorUpdateContext context)
        {
            Model = actorData;

            _statsBar.UpdateModel(actorData);
        }

        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handler)
        {
            _image.sprite = handler.Result;
        }

        public void SetHighlight(HighlightType highlightType)
        {
            _targetingRing.SetHighlight(highlightType);
        }

        private void OnInteraction(InteractionType interactionType)
        {
            // Show enemy stats on hover
            _statsBar.gameObject.UpdateActive(interactionType == InteractionType.Hover);

            Interacted?.Invoke(this, interactionType);
        }
    }
}