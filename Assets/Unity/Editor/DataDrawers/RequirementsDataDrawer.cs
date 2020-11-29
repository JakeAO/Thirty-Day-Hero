using System;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using SadPumpkin.Util.Context;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class RequirementsDataDrawer : IDataDrawer<IRequirementCalc>
    {
        public void DrawGUI(IRequirementCalc value, IContext context, Action<IRequirementCalc> lateRef)
        {
            GUILayout.Label("Not Yet Implemented", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
        }
    }
}