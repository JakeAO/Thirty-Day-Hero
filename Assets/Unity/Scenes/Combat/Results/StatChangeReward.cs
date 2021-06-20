using TMPro;
using Unity.Extensions;
using UnityEngine;

namespace Unity.Scenes.Combat.Results
{
    public class StatChangeReward : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;

        public void SetValue(int statGained)
        {
            _label.text = $"+{statGained}";

            this.UpdateActive(statGained != 0);
        }
    }
}