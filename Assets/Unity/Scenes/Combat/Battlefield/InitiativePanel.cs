using System.Collections.Generic;
using Core.Actors;
using Unity.Extensions;
using Unity.Scenes.Combat.Etc;
using Unity.Scenes.Shared.Entities;
using UnityEngine;

namespace Unity.Scenes.Combat.Battlefield
{
    public class InitiativePanel : MonoBehaviour
    {
        [SerializeField] private CombatInitiativeView _initiativeViewPrefab;
        [SerializeField] private RectTransform _initiativeParent;

        private readonly List<CombatInitiativeView> _initiativeViews = new List<CombatInitiativeView>(10);

        public void UpdateInitiativePreview(IReadOnlyList<ICharacterActor> orderedActors)
        {
            while (_initiativeViews.Count < orderedActors.Count)
            {
                var view = Instantiate(_initiativeViewPrefab, _initiativeParent); // TODO pooling
                view.UpdateActive(false);
                _initiativeViews.Add(view);
            }

            for (int i = 0; i < _initiativeViews.Count; i++)
            {
                if (i < orderedActors.Count)
                {
                    if (_initiativeViews[i].Model?.Id == orderedActors[i].Id)
                    {
                        _initiativeViews[i].UpdateModel(orderedActors[i], ActorUpdateContext.DEFAULT);
                    }
                    else
                    {
                        _initiativeViews[i].InitializeModel(orderedActors[i]);
                    }

                    _initiativeViews[i].UpdateActive(true);
                }
                else
                {
                    _initiativeViews[i].UpdateActive(false);
                }
            }
        }

        public void HighlightActor(uint actorId, HighlightType highlightType)
        {
            foreach (var view in _initiativeViews)
            {
                if (view == null || view.Model == null)
                    continue;

                if (view.Model.Id == actorId)
                {
                    view.SetHighlight(highlightType);
                }
                else
                {
                    view.SetHighlight(HighlightType.None);
                }
            }
        }
    }
}