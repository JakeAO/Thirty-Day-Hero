﻿using System.Linq;
using Core.EventOptions;
using Core.States;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine.States;
using UnityEngine;

namespace Unity.Scenes
{
    public abstract class SceneRootBase<T> : MonoBehaviour, ISceneRoot where T : ITDHState
    {
        public IContext SharedContext { get; protected set; }
        public T State { get; protected set; }

        public void InjectContext(IContext context)
        {
            SharedContext = context;
            State = (T) context.Get<IState>();

            OnInject();
        }

        public void Dispose()
        {
            OnDispose();

            SharedContext = null;
            State = default;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label($"Debug Flow ({GetType().Name})", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});

                GUILayout.BeginVertical(GUI.skin.box);
                {
                    OnGUIContentForState();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    if (State != null)
                    {
                        GUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            foreach (var eventGroup in State
                                .GetOptions()
                                .OrderBy(x => x)
                                .GroupBy(x => x.Category))
                            {
                                GUILayout.BeginVertical();
                                {
                                    // Category Header
                                    if (!string.IsNullOrWhiteSpace(eventGroup.Key))
                                    {
                                        GUILayout.Label(eventGroup.Key, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold, alignment = TextAnchor.LowerCenter});
                                    }

                                    GUILayout.BeginVertical(GUI.skin.box);
                                    {
                                        // Category Contents
                                        foreach (IEventOption eventOption in eventGroup)
                                        {
                                            if (eventOption.Disabled)
                                            {
                                                GUI.enabled = false;
                                                GUI.color = Color.grey;
                                            }

                                            if (eventOption.Context != null)
                                            {
                                                GUILayout.BeginVertical(GUI.skin.box);
                                                {
                                                    OnGUIContentForContext(eventOption.Context);
                                                }
                                                GUILayout.EndVertical();
                                            }

                                            if (!eventOption.Disabled)
                                            {
                                                GUI.color = Color.green;
                                            }

                                            if (GUILayout.Button(eventOption.Text))
                                            {
                                                eventOption.SelectOption?.Invoke();
                                            }

                                            GUI.color = Color.white;
                                            GUI.enabled = true;
                                        }
                                    }
                                    GUILayout.EndVertical();
                                }
                                GUILayout.EndVertical();
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUI.color = Color.red;
                        GUILayout.Label("ERROR");
                        GUILayout.Label($"Scene ({GetType()} does not have a State {typeof(T)} set!");
                        GUI.color = Color.grey;
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }

        protected virtual void OnInject()
        {
        }

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnGUIContentForState()
        {

        }

        protected virtual void OnGUIContentForContext(object context)
        {

        }
    }
}