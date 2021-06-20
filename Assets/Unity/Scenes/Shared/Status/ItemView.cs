using Core.Items;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes.Shared.Status
{
    public class ItemView : MonoBehaviour, IItemView
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;

        public void UpdateModel(IItem item)
        {
            if (item != null)
            {
                _name.text = item.Name;

                Addressables.LoadAssetAsync<Sprite>(item.ArtPath).Completed += OnSpriteLoaded;
            }
            else
            {
                _name.text = string.Empty;
                _icon.sprite = null;
            }
        }

        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
        {
            _icon.sprite = handle.Result;
        }
    }
}