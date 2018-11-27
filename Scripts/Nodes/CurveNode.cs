using UnityEngine;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Events;
using GeoTetra.GTBuilder.Gizmo;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GeoTetra.GTBuilder.Nodes
{
    public class CurveNode : BuilderNode
    {
#if UNITY_EDITOR
        [SerializeField]
        private CurvePrimitive _curvePrimitive;

        [SerializeField]
        private CurvePrimitiveEvent _validated = new CurvePrimitiveEvent();

        private const string _leftHandleName = "CurveLeftHandle";
        private const string _rightHandleName = "CurveRightHandle";
        private const string _positionHandleName = "CurvePositionHandle";

        private SerializedProperty _curvePrimitiveProperty;
        private SerializedProperty _curveHandeListProperty;

        //TODO ALL OF THIS EDITOR STUFF SHOULD BE IN A DIFFERENT CLASS
        protected override void OnEnable()
        {
            base.OnEnable();
            _curvePrimitiveProperty = SerializedBuilderNode.FindProperty("_curvePrimitive");
            _curveHandeListProperty = _curvePrimitiveProperty.FindPropertyRelative("_handles");
        }

        public override void OnSelected()
        {
            base.OnSelected();
            GizmoSelection.Instance.ClearSelectablePoints();
            for (int i = 0; i < _curveHandeListProperty.arraySize; ++i)
            {
                SerializedProperty position = _curveHandeListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Position");
                SerializedProperty leftHandlePosition = _curveHandeListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("LeftHandlePosition");
                SerializedProperty rightHandlePosition = _curveHandeListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("RightHandlePosition");
                
                GizmoSelection.Instance.AddSelectablePoint(new SeralizedSelectablePoint(position, Color.white, (p) =>
                {
                    leftHandlePosition.vector3Value += p.DeltaPosition;
                    rightHandlePosition.vector3Value += p.DeltaPosition;
                }));
                if (_curvePrimitive.Function == CurveFunction.CubicBezier)
                {
                    GizmoSelection.Instance.AddSelectablePoint(new SeralizedSelectablePoint(leftHandlePosition, Color.red, (p) =>
                    {
                        Vector3 delta = leftHandlePosition.vector3Value - position.vector3Value;
                        rightHandlePosition.vector3Value = position.vector3Value - delta;
                    }));
                    GizmoSelection.Instance.AddSelectablePoint(new SeralizedSelectablePoint(rightHandlePosition, Color.blue, (p) =>
                    {
                        Vector3 delta = position.vector3Value - rightHandlePosition.vector3Value;
                        leftHandlePosition.vector3Value = position.vector3Value + delta;
                    }));
                }
            }
        }

        public override void OnDeselected()
        {
            base.OnDeselected();
            GizmoSelection.Instance.ClearSelectablePoints();
        }

        public override void OnChangeOccured()
        {
            Debug.Log("Curve node change");
            if (_validated != null)
            {
                _validated.Invoke(_curvePrimitive);
            }
        }

        public override void OnRenderObject()
        {
        }

        public override void OnSceneGUISelected()
        {
            SerializedBuilderNode.Update();

//            SerializedProperty curvePrimitive = SerializedBuilderNode.FindProperty("_curvePrimitive");
//            SerializedProperty curveHandleList = curvePrimitive.FindPropertyRelative("_handles");

//            OnChangeOccured();
            
            Handles.BeginGUI();

            GUILayout.Window(2, new Rect(0, 20, 100, 100), (id) =>
            {
                if (GUILayout.Button("Add"))
                {
                    _curvePrimitive.Handles.Add(new CurveHandle());
                }
                if (GUILayout.Button("Delete"))
                {
                    _curvePrimitive.Handles.RemoveAt(_curvePrimitive.Handles.Count - 1);
                }
                if (TakeOverSceneInput)
                {
                    if (GUILayout.Button("Close Edit"))
                    {
                        TakeOverSceneInput = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Edit"))
                    {
                        TakeOverSceneInput = true;
                    }
                }

            }, "Curve");

            Handles.EndGUI();

            _curvePrimitive.DrawEditorLine(4, Color.white);

            SerializedBuilderNode.ApplyModifiedProperties();
        }

        private int ParseControlNametoIndex(string name)
        {
            int value = -1;
            string[] split = name.Split('#');
            if (split.Length == 2 && split[0] == _positionHandleName)
            {
                value = int.Parse(split[1]);
            }
            return value;
        }

        private Vector3 EditorPrefsSnap()
        {
            return new Vector3(EditorPrefs.GetFloat("MoveSnapX"), EditorPrefs.GetFloat("MoveSnapY"), EditorPrefs.GetFloat("MoveSnapZ"));
        }
#endif
    }
}