using System;
using SadPumpkin.Util.CombatEngine.TargetCalculators;
using SadPumpkin.Util.Context;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class TargetDataDrawer : IDataDrawer<ITargetCalc>
    {
        public void DrawGUI(ITargetCalc value, IContext context, Action<ITargetCalc> lateRef)
        {
            GUILayout.Label("Not Yet Implemented", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
        }
    }
}