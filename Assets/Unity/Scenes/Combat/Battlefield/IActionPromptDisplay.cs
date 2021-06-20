using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;

namespace Unity.Scenes.Combat.Battlefield
{
    public interface IActionPromptDisplay
    {
        void ShowActionPrompt(uint actorId, IReadOnlyList<(IAction, bool)> actions, Action<IAction> clicked);

        void HideActionPrompt();
    }
}