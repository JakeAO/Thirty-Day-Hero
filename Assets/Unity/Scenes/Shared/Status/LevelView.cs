using Core.Actors;
using Core.Etc;
using TMPro;
using UnityEngine;

namespace Unity.Scenes.Shared.Status
{
    public class LevelView : MonoBehaviour, ILevelView
    {
        [SerializeField] private TMP_Text _levelLabel;
        [SerializeField] private TMP_Text _expLabel;

        public void UpdateModel(ICharacterActor actorData)
        {
            _levelLabel.text = actorData.Stats[StatType.LVL].ToString();
            _expLabel.text = actorData.Stats[StatType.EXP].ToString();
        }
    }
}