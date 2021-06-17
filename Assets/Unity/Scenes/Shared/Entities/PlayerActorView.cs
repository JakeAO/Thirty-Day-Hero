using Core.Actors;
using Core.Actors.Player;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes.Shared.Entities
{
    public class PlayerActorView : MonoBehaviour, IActorView
    {
        [SerializeField] private Image _actorImage;

        private IPlayerCharacterActor _model;

        public IPlayerCharacterActor Model => _model;

        public void InitializeModel(IPlayerCharacterActor actorData)
        {
            _model = actorData;

            UpdateActorImage();
        }

        public void UpdateModel(IPlayerCharacterActor actorData)
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
        void IActorView.InitializeModel(ICharacterActor actorData) => InitializeModel((IPlayerCharacterActor) actorData);
        void IActorView.UpdateModel(ICharacterActor actorData) => UpdateModel((IPlayerCharacterActor) actorData);

        #endregion
    }
}