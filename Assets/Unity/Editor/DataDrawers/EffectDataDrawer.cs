using System;
using SadPumpkin.Util.CombatEngine.EffectCalculators;
using SadPumpkin.Util.Context;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class EffectDataDrawer : IDataDrawer<IEffectCalc>
    {
        public void DrawGUI(IEffectCalc value, IContext context, Action<IEffectCalc> lateRef)
        {
            GUILayout.Label("Not Yet Implemented", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
        }
    }
}