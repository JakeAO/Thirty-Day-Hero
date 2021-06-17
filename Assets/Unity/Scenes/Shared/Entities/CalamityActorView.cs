using Core.Actors;
using Core.Actors.Calamity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes.Shared.Entities
{
    public class CalamityActorView : MonoBehaviour, IActorView
    {
        [SerializeField] private Image _actorImage;

        private ICalamityCharacterActor _model;

        public ICalamityCharacterActor Model => _model;

        public void InitializeModel(ICalamityCharacterActor actorData)
        {
            _model = actorData;

            UpdateActorImage();
        }

        public void UpdateModel(ICalamityCharacterActor actorData)
        {
            _model = actorData;

            UpdateActorImage();
        }

        private void UpdateActorImage()
        {
            Addressables.LoadAssetAsync<Sprite>(_model.Class.ArtPath).Completed += OnClassImageLoaded;
        }

        private void OnClassImageLoaded(AsyncOperationHandle<Sprite> handle)
        {
            _actorImage.sprite = handle.Result;
        }

        #region IActorView

        ICharacterActor IActorView.Model => _model;
        void IActorView.InitializeModel(ICharacterActor actorData) => InitializeModel((ICalamityCharacterActor) actorData);
        void IActorView.UpdateModel(ICharacterActor actorData) => UpdateModel((ICalamityCharacterActor) actorData);

        #endregion
    }
}