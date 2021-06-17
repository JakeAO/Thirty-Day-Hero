using System;
using Core.Etc;
using Core.Items;
using Core.States.Combat;
using Core.States.Combat.Events;
using Core.Wrappers;
using SadPumpkin.Util.Context;
using Unity.Scenes.Shared.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class CombatResultsUxEvent : BaseCombatUxEvent
    {
        private readonly CombatResultsEvent _eventData;
        private readonly IActorViewManager _actorViewManager;
        private readonly CombatDataWrapper _combatDataWrapper;
        private readonly PartyDataWrapper _partyDataWrapper;
        
        public CombatResultsUxEvent(CombatResultsEvent eventData, IContext activeContext)
        {
            _eventData = eventData;
            _actorViewManager = activeContext.Get<IActorViewManager>();
            _combatDataWrapper = activeContext.Get<CombatDataWrapper>();
            _partyDataWrapper = activeContext.Get<PartyDataWrapper>();

            Assert.IsNotNull(_eventData);
            Assert.IsNotNull(_actorViewManager);
            Assert.IsNotNull(_combatDataWrapper);
            Assert.IsNotNull(_partyDataWrapper);
        }

        protected override void OnRun()
        {
            // TODO
            Debug.Log($"{nameof(CombatResultsUxEvent)} executed. Success: {_eventData.Results.Success}");
            if (_eventData.Results.Success)
            {
                Debug.Log($"Rewards...\n" +
                          $"   Exp: {_eventData.Results.ExpReward}\n" +
                          $"   Gold: {_eventData.Results.GoldReward}");
                foreach (IItem item in _eventData.Results.ItemReward)
                {
                    Debug.Log($"Gained Item: {item.Name} ({item.Id})");
                }

                foreach (var statChangeKvp in _eventData.Results.StatChanges)
                {
                    uint actorId = statChangeKvp.Key;
                    int[] changes = statChangeKvp.Value;
                    string statChangeText = string.Empty;

                    foreach (var enumValue in Enum.GetValues(typeof(StatType)))
                    {
                        StatType statType = (StatType) enumValue;
                        int index = (int) statType;
                        int change = 0;
                        switch (statType)
                        {
                            case StatType.Invalid:
                            case StatType.HP:
                            case StatType.STA:
                                // nah
                                break;
                            case StatType.EXP:
                                change = changes[index] + changes[(int) StatType.LVL] * 100;
                                break;
                            default:
                                change = changes[index];
                                break;
                        }

                        if (change != 0)
                        {
                            statChangeText += $"\n   {statType} + {change}";
                        }
                    }

                    Debug.Log($"Actor id {actorId}'s stats changed...{statChangeText}");
                }
            }

            Complete();
        }

        protected override void OnTickUpdate(float deltaTimeMs)
        {
        }
    }
}