using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Graph;
using GeoTetra.GTBuilder.Gizmo;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace GeoTetra.GTBuilder.Nodes
{
    [System.Serializable]
    public abstract class BuilderNode : ScriptableObject
    {
#if UNITY_EDITOR
        [HideInInspector]
        [SerializeField]
        List<UnityEventLink> _unityEventLinkList = new List<UnityEventLink>();

        [HideInInspector]
        [SerializeField]
        Rect _windowRect = new Rect(0, 0, 300, 0);

        public List<UnityEventLink> UnityEventLinkList { get { return _unityEventLinkList; } }
        public Rect WindowRect { get { return _windowRect; } set { _windowRect = value; } }
        public BuilderGraph ParentBuilderGraph { get; private set; }
        public bool TakeOverSceneInput { get; protected set; }
        public SerializedObject SerializedBuilderNode { get; private set; }

        /// <summary>
        /// Called when node is first initialized. 
        /// (e.g. called on create, called after recompiled)
        /// </summary>
        protected virtual void OnEnable()
        {
            SerializedBuilderNode = new SerializedObject(this);
        }

        /// <summary>
        /// Called when any changes to Serialized Fields are made.
        /// </summary>
        public virtual void OnChangeOccured()
        {
        }

        public void DrawNode(BuilderGraph builderGraph)
        {
            ParentBuilderGraph = builderGraph;
            //		_windowRect.height = 0;
            //		_windowRect.width = 300;
            _windowRect = GUILayout.Window(GetHashCode(), _windowRect, DrawWindow, ToString());
            if (_windowRect.x < 0)
            {
                _windowRect.x = 0;
            }
            if (_windowRect.y < 0)
            {
                _windowRect.y = 0;
            }
        }

        public virtual void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            SerializedBuilderNode.Update();
            DrawDefaultWindow();
            SerializedBuilderNode.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                OnChangeOccured();
            }
        }

        public virtual void OnSelected()
        {
            GizmoSelection.Instance.ClearGizmo();
            if (ParentBuilderGraph.SelectedNode != null)
            {
                ParentBuilderGraph.SelectedNode.OnDeselected();
            }
            ParentBuilderGraph.SelectedNode = this;
            SceneView.RepaintAll();
        }

        public virtual void OnDeselected()
        {
            TakeOverSceneInput = false;
            Tools.hidden = false;
        }

        /// <summary>
        /// Called every time any input occurs in SceneView
        /// </summary>
        public virtual void OnSceneGUISelected()
        {
        }

        /// <summary>
        /// Called every time Scene view needs to be re-rendered.
        /// </summary>
        public virtual void OnRenderObject()
        {
        }

        public virtual void OnDrawGizmos()
        {
        }

        public virtual void OnDrawGizmosSelected()
        {
        }

        protected void DrawDefaultWindow()
        {
            //gather UnityEvents through reflection and store their name
            _unityEventLinkList.Clear();
            foreach (FieldInfo fieldInfo in SerializedBuilderNode.targetObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (fieldInfo.FieldType.IsSubclassOf(typeof(UnityEventBase)))
                {
                    UnityEventLink unityEventLink = new UnityEventLink();
                    unityEventLink.StartFieldName = fieldInfo.Name;
                    _unityEventLinkList.Add(unityEventLink);
                }
            }

            SerializedProperty iterator = SerializedBuilderNode.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
                UnityEventLink unityEventLink = _unityEventLinkList.Find((u) => u.StartFieldName == iterator.name);
                if (unityEventLink != null)
                {
                    unityEventLink.StartRect = GUILayoutUtility.GetLastRect();
                    SerializedProperty persistentCalls = iterator.FindPropertyRelative("m_PersistentCalls");
                    SerializedProperty calls = persistentCalls.FindPropertyRelative("m_Calls");
                    unityEventLink.TargetObjectList.Clear();
                    unityEventLink.MethodNameList.Clear();
                    for (int i = 0; i < calls.arraySize; ++i)
                    {
                        unityEventLink.TargetObjectList.Add(calls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Target").objectReferenceValue);
                        unityEventLink.MethodNameList.Add(calls.GetArrayElementAtIndex(i).FindPropertyRelative("m_MethodName").stringValue);
                    }
                }
                enterChildren = false;
            }

            if (Event.current.type == EventType.Repaint)
            {
                _windowRect.height = GUILayoutUtility.GetLastRect().yMax;
            }
        }

        private void DrawWindow(int id)
        {
            if (Event.current.button == 0 && Event.current.type == EventType.MouseUp)
            {
                OnSelected();
            }

            if (Event.current.button == 1 && Event.current.type == EventType.MouseUp)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Delete"), false, () =>
                {
                    ParentBuilderGraph.LogicNodes.Remove(this);
                    DestroyImmediate(this);
                });
                menu.ShowAsContext();
                Event.current.Use();
            }

            if (Event.current.button == 0 && Event.current.modifiers == EventModifiers.Alt && Event.current.type == EventType.MouseDown)
            {
                Debug.Log("HERE " + ToString());
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new Object[] { this };
                DragAndDrop.StartDrag(ToString());
                Event.current.Use();
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            OnGUI();
        }
#endif
    }
}