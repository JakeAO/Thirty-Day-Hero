using Unity.Interface;
using UnityEditor;
using UnityEditor.UI;

namespace Unity.Editor.CustomInspectors
{
    [CustomEditor(typeof(ButtonWithLabel))]
    public class ButtonWithLabelEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_label"));
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}