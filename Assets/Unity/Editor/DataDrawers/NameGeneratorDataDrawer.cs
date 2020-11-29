using System;
using System.Collections.Generic;
using Core.Costs;
using Core.Etc;
using Core.Naming;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.Context;
using Unity.Editor.WindowUtilities;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class NameGeneratorDataDrawer : IDataDrawer<INameGenerator>
    {
        public void DrawGUI(INameGenerator value, IContext context, Action<INameGenerator> lateRef)
        {
            // Settings Change
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(value.GetType().Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                }
                GUILayout.EndHorizontal();

                switch (value)
                {
                    case ListNameGenerator listNameGenerator:
                    {
                        for (int i = 0; i < listNameGenerator.Names.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            listNameGenerator.Names[i] = EditorGUILayout.TextField(listNameGenerator.Names[i]);
                            GUI.color = Color.red;
                            if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                            {
                                listNameGenerator.Names.RemoveAt(i);
                                i--;
                            }
                            GUI.color = Color.white;
                            GUILayout.EndHorizontal();
                        }

                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        GUI.color = Color.green;
                        if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                        {
                            listNameGenerator.Names.Add(string.Empty);
                        }
                        GUI.color = Color.white;
                        GUILayout.EndHorizontal();
                        
                        break;
                    }
                    case FileNameGenerator fileNameGenerator:
                    {
                        GUILayout.BeginHorizontal();
                        fileNameGenerator.FilePath = EditorGUILayout.TextField("File:", fileNameGenerator.FilePath);
                        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(fileNameGenerator.FilePath);
                        TextAsset newTextAsset = (TextAsset) EditorGUILayout.ObjectField(textAsset, typeof(TextAsset), false, GUILayout.ExpandWidth(false));
                        if (textAsset != newTextAsset)
                        {
                            fileNameGenerator.FilePath = AssetDatabase.GetAssetPath(newTextAsset);
                        }
                        GUILayout.EndHorizontal();
                        break;
                    }
                    case NullNameGenerator nullNameGenerator:
                    {
                        EditorGUILayout.HelpBox(
                            "No configuration available.",
                            MessageType.Info);
                        break;
                    }
                    default:
                    {
                        EditorGUILayout.HelpBox(
                            "Unhandled Type!.",
                            MessageType.Error);
                        break;
                    }
                }
            }
            GUILayout.EndVertical();

            // Type Change
            if (GUILayout.Button("[CHANGE]", GUILayout.ExpandWidth(false)))
            {
                IReadOnlyDictionary<Type, Func<INameGenerator>> implementations = ImplementationFinder.Find<INameGenerator>();

                GenericMenu gm = new GenericMenu();
                if (implementations.Count == 0)
                {
                    gm.AddDisabledItem(new GUIContent($"No {nameof(INameGenerator)} implementations found"));
                }
                else
                {
                    foreach (var implementationKvp in implementations)
                    {
                        void SetImplementation(object funcObj)
                        {
                            if (funcObj is Func<INameGenerator> func)
                                lateRef?.Invoke(func());
                        }

                        Func<INameGenerator> funcCache = implementationKvp.Value;
                        GUIContent content = new GUIContent(implementationKvp.Key.Name);
                        bool active = value.GetType() == implementationKvp.Key;

                        gm.AddItem(content, active, SetImplementation, funcCache);
                    }
                }
                gm.ShowAsContext();
            }
        }
    }
}