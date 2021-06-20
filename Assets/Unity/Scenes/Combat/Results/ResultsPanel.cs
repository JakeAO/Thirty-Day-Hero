using System;
using Core.CombatSettings;
using Core.Wrappers;
using Unity.Extensions;
using UnityEngine;

namespace Unity.Scenes.Combat.Results
{
    public class ResultsPanel : MonoBehaviour
    {
        [SerializeField] private DefeatSubPanel _defeatSubPanel;
        [SerializeField] private VictorySubPanel _victorySubPanel;

        public void Show(
            CombatResults results,
            PartyDataWrapper partyDataWrapper,
            Action done)
        {
            this.UpdateActive(true);
            
            if (results.Success)
            {
                _victorySubPanel.Show(results, partyDataWrapper, done);
                _victorySubPanel.UpdateActive(true);
            }
            else
            {
                _defeatSubPanel.Show(done);
                _defeatSubPanel.UpdateActive(true);
            }
        }
    }
}