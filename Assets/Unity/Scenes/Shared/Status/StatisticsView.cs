using Core.Actors;
using Core.Etc;
using TMPro;
using UnityEngine;

namespace Unity.Scenes.Shared.Status
{
    public class StatisticsView : MonoBehaviour, IStatisticsView
    {
        [SerializeField] private TMP_Text _strLabel;
        [SerializeField] private TMP_Text _dexLabel;
        [SerializeField] private TMP_Text _conLabel;
        [SerializeField] private TMP_Text _intLabel;
        [SerializeField] private TMP_Text _magLabel;
        [SerializeField] private TMP_Text _chaLabel;

        public void UpdateModel(ICharacterActor actorData)
        {
            _strLabel.text = actorData.Stats[StatType.STR].ToString();
            _dexLabel.text = actorData.Stats[StatType.DEX].ToString();
            _conLabel.text = actorData.Stats[StatType.CON].ToString();
            _intLabel.text = actorData.Stats[StatType.INT].ToString();
            _magLabel.text = actorData.Stats[StatType.MAG].ToString();
            _chaLabel.text = actorData.Stats[StatType.CHA].ToString();
        }
    }
}