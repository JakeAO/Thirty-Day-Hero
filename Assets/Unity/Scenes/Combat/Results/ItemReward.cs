using Core.Items;
using TMPro;
using Unity.Extensions;
using Unity.Scenes.Shared.Status;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes.Combat.Results
{
    public class ItemReward : MonoBehaviour, IItemView
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;

        public void UpdateModel(IItem item)
        {
            _label.text = item.Name;

            if (!string.IsNullOrWhiteSpace(item.ArtPath))
            {
                Addressables.LoadAssetAsync<Sprite>(item.ArtPath).Completed += OnSpriteLoaded;
            }
            else
            {
                _icon.sprite = SpriteExtensions.RedSprite;
            }
        }

        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
        {
            _icon.sprite = handle.Result;
        }
    }
}