using System;
using System.Collections;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Gizmo;
using GeoTetra.GTLogicGraph;
using UnityEditor;
using UnityEngine;

namespace GeoTetra.GTBuilder.Component
{
    [CustomEditor(typeof(Curve))]
    public class CurveEditor : Editor
    {
        private SerializedProperty _curvePrimitiveProperty;
        private SerializedProperty _curveHandeListProperty;

        private bool _takeOverSceneInput;
        private Curve _curve;

        private void OnEnable()
        {
            _curve = target as Curve;
            _curvePrimitiveProperty = serializedObject.FindProperty("_curvePrimitive");
            _curveHandeListProperty = _curvePrimitiveProperty.FindPropertyRelative("_handles");
            
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
                    _curve.OnChange();
                }));
                if (_curve.Primitive.Function == CurveFunction.CubicBezier)
                {
                    GizmoSelection.Instance.AddSelectablePoint(new SeralizedSelectablePoint(leftHandlePosition, Color.red, (p) =>
                    {
                        Vector3 delta = leftHandlePosition.vector3Value - position.vector3Value;
                        rightHandlePosition.vector3Value = position.vector3Value - delta;
                        _curve.OnChange();
                    }));
                    GizmoSelection.Instance.AddSelectablePoint(new SeralizedSelectablePoint(rightHandlePosition, Color.blue, (p) =>
                    {
                        Vector3 delta = position.vector3Value - rightHandlePosition.vector3Value;
                        leftHandlePosition.vector3Value = position.vector3Value + delta;
                        _curve.OnChange();
                    }));
                }
            }
        }

        private void OnDisable()
        {
            GizmoSelection.Instance.ClearSelectablePoints();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            if (_takeOverSceneInput)
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


            serializedObject.Update();

            GizmoSelection.Instance.OnSceneGUI();
            
            Handles.BeginGUI();

            GUILayout.Window(2, new Rect(0, 20, 100, 100), (id) =>
            {
                if (GUILayout.Button("Add"))
                {
                    _curve.Primitive.Handles.Add(new CurveHandle());
                }

                if (GUILayout.Button("Delete"))
                {
                    _curve.Primitive.Handles.RemoveAt(_curve.Primitive.Handles.Count - 1);
                }

                if (_takeOverSceneInput)
                {
                    if (GUILayout.Button("Close Edit"))
                    {
                        _takeOverSceneInput = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Edit"))
                    {
                        _takeOverSceneInput = true;
                    }
                }
            }, "Curve");

            Handles.EndGUI();

            _curve.Primitive.DrawEditorLine(4, Color.white);

            serializedObject.ApplyModifiedProperties();
            
            GizmoSelection.Instance.RenderGizmos();
        }
    }
}