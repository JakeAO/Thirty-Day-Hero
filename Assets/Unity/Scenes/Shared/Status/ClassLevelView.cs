using Core.Actors;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes.Shared.Status
{
    public class ClassLevelView : MonoBehaviour, IClassLevelView
    {
        [SerializeField] private Image _classIcon;
        [SerializeField] private TMP_Text _className;
        [SerializeField] private LevelView _levelView;

        public void UpdateModel(ICharacterActor actorData)
        {
            _className.text = actorData.Class.Name;
            _levelView.UpdateModel(actorData);

            Addressables.LoadAssetAsync<Sprite>(actorData.Class.ArtPath).Completed += OnSpriteLoaded;
        }

        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
        {
            _classIcon.sprite = handle.Result;
        }
    }
}