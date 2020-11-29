using System;
using Core.Abilities;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using SadPumpkin.Util.CombatEngine.TargetCalculators;
using SadPumpkin.Util.Context;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class AbilityDataDrawer : IDataDrawer<Ability>
    {
        private IDataDrawer<IRequirementCalc> _requirementDrawer = null;
        private IDataDrawer<ICostCalc> _costDrawer = null;
        private IDataDrawer<ITargetCalc> _targetDrawer = null;
        private IDataDrawer<IEffectCalc> _effectDrawer = null;

        public AbilityDataDrawer()
        {
            _requirementDrawer = DataDrawerFinder.Find<IRequirementCalc>();
            _costDrawer = DataDrawerFinder.Find<ICostCalc>();
            _targetDrawer = DataDrawerFinder.Find<ITargetCalc>();
            _effectDrawer = DataDrawerFinder.Find<IEffectCalc>();
        }

        public void DrawGUI(Ability value, IContext context, Action<Ability> lateRef)
        {
            value.Id = (uint) EditorGUILayout.IntField("Id:", (int) value.Id);
            value.Name = EditorGUILayout.TextField("Name:", value.Name);
            value.Desc = EditorGUILayout.TextField("Description:", value.Desc);
            value.Speed = (uint) EditorGUILayout.IntField("Speed:", (int) value.Speed);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Requirements:", GUILayout.ExpandWidth(false));
            if (_requirementDrawer != null)
            {
                _requirementDrawer.DrawGUI(value.Requirements, context, newCalc => value.Requirements = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "No corresponding DataDrawer could be found for IRequirementCalc!",
                    MessageType.Error);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Cost:", GUILayout.ExpandWidth(false));
            if (_costDrawer != null)
            {
                _costDrawer.DrawGUI(value.Cost, context, newCalc => value.Cost = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "No corresponding DataDrawer could be found for ICostCalc!",
                    MessageType.Error);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Target:", GUILayout.ExpandWidth(false));
            if (_targetDrawer != null)
            {
                _targetDrawer.DrawGUI(value.Target, context, newCalc => value.Target = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "No corresponding DataDrawer could be found for ITargetCalc!",
                    MessageType.Error);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Effect:", GUILayout.ExpandWidth(false));
            if (_effectDrawer != null)
            {
                _effectDrawer.DrawGUI(value.Effect, context, newCalc => value.Effect = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "No corresponding DataDrawer could be found for IEffectCalc!",
                    MessageType.Error);
            }
            GUILayout.EndHorizontal();
        }
    }
}