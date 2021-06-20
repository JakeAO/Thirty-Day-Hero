using System;
using Unity.Scenes.Combat.Etc;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scenes.Combat.Battlefield
{
    public class TargetingIndicator : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        [SerializeField] private Color _noneColor = Color.clear;
        [SerializeField] private Color _selectableColor = Color.Lerp(Color.blue, Color.clear, 0.5f);
        [SerializeField] private Color _selectedColor = Color.Lerp(Color.blue, Color.clear, 0.25f);
        [SerializeField] private Color _blockedColor = Color.Lerp(Color.red, Color.clear, 0.75f);
        [SerializeField] private Color _activeColor = Color.Lerp(Color.green, Color.clear, 0.5f);

        public void SetHighlight(HighlightType highlightType)
        {
            switch (highlightType)
            {
                case HighlightType.None:
                    _image.color = _noneColor;
                    break;
                case HighlightType.Selectable:
                    _image.color = _selectableColor;
                    break;
                case HighlightType.Selected:
                    _image.color = _selectedColor;
                    break;
                case HighlightType.BlockedSelection:
                    _image.color = _blockedColor;
                    break;
                case HighlightType.Active:
                    _image.color = _activeColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(highlightType), highlightType, null);
            }
        }
    }
}