using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Interface
{
    public class ButtonWithLabel : Button
    {
        [SerializeField] private TMP_Text _label;

        public void SetText(string text)
        {
            _label.text = text;
        }

        public void SetText(string format, params object[] args)
        {
            _label.text = string.Format(format, args);
        }
    }
}