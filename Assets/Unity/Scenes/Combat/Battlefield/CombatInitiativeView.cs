using System;
using Core.Actors;
using TMPro;
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
    public class CombatInitiativeView : MonoBehaviour, IActorView, IHighlightableView, IInteractableView<IActorView>
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private TargetingIndicator _targetingIndicator;

        public event Action<IActorView, InteractionType> Interacted;

        public ICharacterActor Model { get; private set; }

        private void Awake()
        {
            GetComponent<UnityInteractionHandler>().Interacted += OnInteraction;
        }

        public void InitializeModel(ICharacterActor actorData)
        {
            Model = actorData;

            _label.text = actorData.Name;
            _targetingIndicator.SetHighlight(HighlightType.None);

            Addressables.LoadAssetAsync<Sprite>(actorData.Class.ArtPath).Completed += OnSpriteLoaded;
        }

        public void UpdateModel(ICharacterActor actorData, ActorUpdateContext context)
        {
            Model = actorData;
        }

        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
        {
            _image.sprite = handle.Result;
        }

        public void SetHighlight(HighlightType highlightType)
        {
            _targetingIndicator.SetHighlight(highlightType);
        }

        private void OnInteraction(InteractionType interactionType)
        {
            Interacted?.Invoke(this, interactionType);
        }
    }
}