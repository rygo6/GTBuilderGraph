using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using GeoTetra.GTBuilder.Gizmo;
using GeoTetra.GTBuilder.Nodes;

namespace GeoTetra.GTBuilder.Graph
{
    public class BuilderGraphWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private Vector2 _contentSize;
        private Vector2 _mousePosition;

        [MenuItem("Window/BuilderGraph")]
        private static void Init()
        {
            BuilderGraphWindow window = EditorWindow.GetWindow<BuilderGraphWindow>();
            window.wantsMouseMove = true;
            window.Show();
        }

        private void OnEnable()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        //Must be static to subscribe to delegate
        //draws GUI in scene view
        private static void OnSceneGUI(SceneView sceneview)
        {
            if (BuilderGraphUtility.SelectedBuilderGraph != null && BuilderGraphUtility.SelectedBuilderGraph.SelectedNode != null)
            {
                if (BuilderGraphUtility.SelectedBuilderGraph.SelectedNode.TakeOverSceneInput)
                {
                    //blocks other objects from being selected
                    int controlID = GUIUtility.GetControlID(FocusType.Passive);
                    if (Event.current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(controlID);
                    }
                    Tools.hidden = true;
                }
                else
                {
                    Tools.hidden = false;
                }

                EditorGUI.BeginChangeCheck();
                BuilderGraphUtility.SelectedBuilderGraph.SelectedNode.OnSceneGUISelected();
                if (EditorGUI.EndChangeCheck())
                {
                    BuilderGraphUtility.SelectedBuilderGraph.SelectedNode.OnChangeOccured();
                }

                BuilderGraphUtility.SelectedBuilderGraph.SelectedNode.SerializedBuilderNode.Update();
                GizmoSelection.Instance.OnSceneGUI();
                BuilderGraphUtility.SelectedBuilderGraph.SelectedNode.SerializedBuilderNode.ApplyModifiedProperties();

                //consumes events to the point that window navigation does not work
                //if (Event.current.type != EventType.layout &&
                //    Event.current.type != EventType.repaint)
                //{
                //    //Event.current.Use();
                //}
            }
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.MouseMove)
            {
                _mousePosition = Event.current.mousePosition;
            }

            //All objects deselected
            if (Selection.activeGameObject == null)
            {
                if (BuilderGraphUtility.SelectedBuilderGraph != null)
                {
                    GizmoSelection.Instance.ClearGizmo();
                    BuilderGraphUtility.SelectedBuilderGraph = null;
                }
            }
            else
            {   
                //builder graph selection changed
                BuilderGraph builderGraph = Selection.activeGameObject.GetComponent<BuilderGraph>();
                if (BuilderGraphUtility.SelectedBuilderGraph != null && BuilderGraphUtility.SelectedBuilderGraph != builderGraph)
                {
                    GizmoSelection.Instance.ClearGizmo();
                }
                BuilderGraphUtility.SelectedBuilderGraph = builderGraph;
            }

            if (BuilderGraphUtility.SelectedBuilderGraph != null)
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, true, true);
                //done to expand size so it will scroll
                GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Width(_contentSize.x), GUILayout.Height(_contentSize.y));

                //draw nodes
                _contentSize = Vector2.zero;
                BeginWindows();
                foreach (BuilderNode logicNode in BuilderGraphUtility.SelectedBuilderGraph.LogicNodes)
                {
                    if (logicNode.WindowRect.xMax > _scrollPosition.x &&
                        logicNode.WindowRect.yMax > _scrollPosition.y &&
                        logicNode.WindowRect.xMin < position.size.x + _scrollPosition.x &&
                        logicNode.WindowRect.yMin < position.size.y + _scrollPosition.y)
                    {
                        logicNode.DrawNode(BuilderGraphUtility.SelectedBuilderGraph);
                    }
                    if (logicNode.WindowRect.xMax > _contentSize.x)
                    {
                        _contentSize.x = logicNode.WindowRect.max.x;
                    }
                    if (logicNode.WindowRect.yMax > _contentSize.y)
                    {
                        _contentSize.y = logicNode.WindowRect.max.y;
                    }
                }
                EndWindows();
                Repaint();

                _contentSize.x += 100;
                _contentSize.y += 100;

                //middle click drag
                if (Event.current.button == 2 && Event.current.type == EventType.MouseDrag)
                {
                    _scrollPosition -= Event.current.delta;
                    Event.current.Use();
                }

                //right click menu
                if (Event.current.button == 1 && Event.current.type == EventType.MouseUp)
                {
                    GenericMenu menu = new GenericMenu();
                    foreach (Type nodeType in GetAllTypesFromBase<BuilderNode>())
                    {
                        Type typeCopy = nodeType;
                        menu.AddItem(new GUIContent(nodeType.ToString()), false, () =>
                        {
                            BuilderNode node = ScriptableObject.CreateInstance(typeCopy) as BuilderNode;
                            node.WindowRect = new Rect(
                                _mousePosition.x + _scrollPosition.x,
                                _mousePosition.y + _scrollPosition.y,
                                300, 0);
                            BuilderGraphUtility.SelectedBuilderGraph.LogicNodes.Add(node);
                        });
                    }
                    menu.ShowAsContext();
                    Event.current.Use();
                }

                //deslect node when click in empty space
                if (Event.current.button == 0 && Event.current.type == EventType.MouseUp)
                {
                    GizmoSelection.Instance.ClearGizmo();
                    if (BuilderGraphUtility.SelectedBuilderGraph.SelectedNode != null)
                    {
                        BuilderGraphUtility.SelectedBuilderGraph.SelectedNode.OnDeselected();
                    }
                    BuilderGraphUtility.SelectedBuilderGraph.SelectedNode = null;
                    SceneView.RepaintAll();
                    Event.current.Use();
                }

                //draw logic nodes event links
                foreach (BuilderNode node in BuilderGraphUtility.SelectedBuilderGraph.LogicNodes)
                {
                    foreach (UnityEventLink unityEventLink in node.UnityEventLinkList)
                    {
                        Rect rect = unityEventLink.StartRect;
                        rect.x += node.WindowRect.x;
                        rect.y += node.WindowRect.y;
                        rect.x += node.WindowRect.width;
                        rect.width = 2;
                        EditorGUI.DrawRect(rect, Color.gray);
                        foreach (object target in unityEventLink.TargetObjectList)
                        {
                            if (target != null && target is BuilderNode)
                            {
                                BuilderNode targetComponent = (BuilderNode)target;
                                BuilderNode targetComponentNode = BuilderGraphUtility.SelectedBuilderGraph.LogicNodes.Find((n) => n == targetComponent);
                                Vector3 startPoint = rect.center;
                                Vector3 endPoint = targetComponentNode.WindowRect.center;
                                endPoint.x -= targetComponentNode.WindowRect.width / 2;
                                Vector3 startTangent = startPoint;
                                startTangent.x += 100;
                                Vector3 endTangent = endPoint;
                                endTangent.x -= 100;
                                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.gray, null, 4);
                            }
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
        }

        private void OnSelectionChange()
        {
            Repaint();
        }

        private List<Type> GetAllTypesFromBase<T>()
        {
            return (from t in Assembly.Load("Assembly-CSharp").GetTypes()
                    where t.IsSubclassOf(typeof(T))
                    select t).ToList();
        }
    }
}