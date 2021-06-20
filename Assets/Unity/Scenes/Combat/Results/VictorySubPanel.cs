using System;
using Core.Actors.Player;
using Core.CombatSettings;
using Core.Items;
using Core.Wrappers;
using TMPro;
using Unity.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scenes.Combat.Results
{
    public class VictorySubPanel : MonoBehaviour
    {
        [SerializeField] private Button _doneButton;
        [SerializeField] private GameObject _expRewardObject;
        [SerializeField] private TMP_Text _expRewardLabel;
        [SerializeField] private GameObject _goldRewardObject;
        [SerializeField] private TMP_Text _goldRewardLabel;
        [SerializeField] private RectTransform _itemRewardRoot;
        [SerializeField] private ItemReward _itemRewardPrefab;
        [SerializeField] private RectTransform _statRewardRoot;
        [SerializeField] private StatReward _statRewardPrefab;

        private CombatResults _result;
        private PartyDataWrapper _partyDataWrapper;
        private Action _done;

        public void Show(CombatResults results, PartyDataWrapper partyDataWrapper, Action done)
        {
            _result = results;
            _partyDataWrapper = partyDataWrapper;
            _done = done;

            // TODO make this party pretty? animate? delay? etc?
            _expRewardLabel.text = $"+{_result.ExpReward}";
            _expRewardObject.UpdateActive(_result.ExpReward > 0);

            _goldRewardLabel.text = $"+{_result.GoldReward}";
            _goldRewardObject.UpdateActive(_result.GoldReward > 0);

            foreach (IItem item in _result.ItemReward)
            {
                ItemReward rewardView = Instantiate(_itemRewardPrefab, _itemRewardRoot);
                rewardView.UpdateModel(item);
            }
            _itemRewardRoot.UpdateActive(_result.ItemReward.Count > 0);

            foreach (var statChangeKvp in _result.StatChanges)
            {
                IPlayerCharacterActor statChangeActor = _partyDataWrapper.Characters.Find(x => x.Id == statChangeKvp.Key);
                StatReward rewardView = Instantiate(_statRewardPrefab, _statRewardRoot);
                rewardView.Show(statChangeActor, statChangeKvp.Value);
            }
            _statRewardRoot.UpdateActive(_result.StatChanges.Count > 0);

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }

        private void OnDoneClicked()
        {
            _done?.Invoke();
        }

        private void OnEnable()
        {
            _doneButton.onClick.AddListener(OnDoneClicked);
        }

        private void OnDisable()
        {
            _doneButton.onClick.RemoveAllListeners();
        }
    }
}