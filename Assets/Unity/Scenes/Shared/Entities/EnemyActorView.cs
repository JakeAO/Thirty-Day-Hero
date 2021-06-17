using Core.Actors;
using Core.Actors.Enemy;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes.Shared.Entities
{
    public class EnemyActorView : MonoBehaviour, IActorView
    {
        [SerializeField] private Image _actorImage;

        private IEnemyCharacterActor _model;

        public IEnemyCharacterActor Model => _model;

        public void InitializeModel(IEnemyCharacterActor actorData)
        {
            _model = actorData;

            UpdateActorImage();
        }

        public void UpdateModel(IEnemyCharacterActor actorData)
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
        void IActorView.InitializeModel(ICharacterActor actorData) => InitializeModel((IEnemyCharacterActor) actorData);
        void IActorView.UpdateModel(ICharacterActor actorData) => UpdateModel((IEnemyCharacterActor) actorData);

        #endregion
    }
}