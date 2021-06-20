using System;
using Unity.Scenes.Combat.Etc;

namespace Unity.Scenes.Combat.Battlefield
{
    public interface IInteractableView<out TViewType>
    {
        event Action<TViewType, InteractionType> Interacted;
    }
}