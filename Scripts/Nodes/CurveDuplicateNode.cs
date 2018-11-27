using UnityEngine;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GeoTetra.GTBuilder.Nodes
{
    public class CurveDuplicateNode : BuilderNode
    {
#if UNITY_EDITOR
        [SerializeField]
        Vector3 _globalOffset = Vector3.zero;

        [SerializeField]
        CurvePrimitive _primitive;

        [SerializeField]
        CurvePrimitive _duplicatePrimitive = new CurvePrimitive();

        [SerializeField]
        List<CurveHandle> _offsets = new List<CurveHandle>();

        [SerializeField]
        CurvePrimitiveEvent _validated = new CurvePrimitiveEvent();

        public CurvePrimitive Primitive
        {
            get { return _primitive; }
            set
            {
                _primitive = value;
                Process();
            }
        }

        public override void OnChangeOccured()
        {
            CreateDuplicateCurve();
        }

        public void Process()
        {
            _duplicatePrimitive.Function = _primitive.Function;
            _duplicatePrimitive.Closed = _primitive.Closed;

            while (_offsets.Count < _primitive.Handles.Count)
            {
                _offsets.Add(new CurveHandle());
            }
            while (_offsets.Count > _primitive.Handles.Count)
            {
                _offsets.RemoveAt(_offsets.Count - 1);
            }

            while (_duplicatePrimitive.Handles.Count < _primitive.Handles.Count)
            {
                _duplicatePrimitive.Handles.Add(new CurveHandle());
            }
            while (_duplicatePrimitive.Handles.Count > _primitive.Handles.Count)
            {
                _duplicatePrimitive.Handles.RemoveAt(_offsets.Count - 1);
            }

            CreateDuplicateCurve();
        }

        void CreateDuplicateCurve()
        {
            for (int i = 0; i < _primitive.Handles.Count; ++i)
            {
                _duplicatePrimitive.Handles[i] = new CurveHandle(
                    _primitive.Handles[i].Position + _offsets[i].Position + _globalOffset,
                    _primitive.Handles[i].RightHandlePosition + _offsets[i].RightHandlePosition + _globalOffset,
                    _primitive.Handles[i].LeftHandlePosition + _offsets[i].LeftHandlePosition + _globalOffset);
            }
            if (_validated != null)
            {
                _validated.Invoke(_duplicatePrimitive);
            }
        }

        public override void OnSceneGUISelected()
        {
            SerializedBuilderNode.Update();

            SerializedProperty primitive = SerializedBuilderNode.FindProperty("_primitive");
            SerializedProperty handles = primitive.FindPropertyRelative("_handles");
            SerializedProperty duplicatePrimitive = SerializedBuilderNode.FindProperty("_duplicatePrimitive");
            SerializedProperty duplicateHandles = duplicatePrimitive.FindPropertyRelative("_handles");
            SerializedProperty offsets = SerializedBuilderNode.FindProperty("_offsets");

            SerializedProperty curveFunctionProperty = duplicatePrimitive.FindPropertyRelative("_function");
            CurveFunction curveFunction = (CurveFunction)curveFunctionProperty.enumValueIndex;

            for (int i = 0; i < duplicateHandles.arraySize; ++i)
            {
                Handles.color = Color.red;

                SerializedProperty position = handles.GetArrayElementAtIndex(i).FindPropertyRelative("Position");
                SerializedProperty offsetPosition = offsets.GetArrayElementAtIndex(i).FindPropertyRelative("Position");
                Vector3 priorOffsetPosition = offsetPosition.vector3Value;
                Vector3 newPosition = Handles.FreeMoveHandle(
                                          position.vector3Value + offsetPosition.vector3Value + _globalOffset,
                                          Quaternion.identity,
                                          HandleUtility.GetHandleSize(position.vector3Value) * .1f,
                                          Vector3.one,
                                          Handles.DotCap);
                offsetPosition.vector3Value = newPosition - position.vector3Value - _globalOffset;
                Vector3 offsetDeltaPosition = offsetPosition.vector3Value - priorOffsetPosition;

                if (curveFunction != CurveFunction.Linear)
                {
                    Handles.color = Color.cyan;
                    SerializedProperty leftHandlePosition = handles.GetArrayElementAtIndex(i).FindPropertyRelative("LeftHandlePosition");
                    SerializedProperty offsetLeftHandlePosition = offsets.GetArrayElementAtIndex(i).FindPropertyRelative("LeftHandlePosition");
                    SerializedProperty rightHandlePosition = handles.GetArrayElementAtIndex(i).FindPropertyRelative("RightHandlePosition");
                    SerializedProperty offsetRightHandlePosition = offsets.GetArrayElementAtIndex(i).FindPropertyRelative("RightHandlePosition");

                    Vector3 priorOffsetLeftHandlePosition = offsetLeftHandlePosition.vector3Value;
                    Vector3 newLeftHandlePosition = Handles.FreeMoveHandle(
                                                        leftHandlePosition.vector3Value + offsetLeftHandlePosition.vector3Value + _globalOffset,
                                                        Quaternion.identity,
                                                        HandleUtility.GetHandleSize(leftHandlePosition.vector3Value) * .1f,
                                                        Vector3.one,
                                                        Handles.DotCap);
                    offsetLeftHandlePosition.vector3Value = newLeftHandlePosition - leftHandlePosition.vector3Value - _globalOffset;
                    if (priorOffsetLeftHandlePosition != offsetLeftHandlePosition.vector3Value)
                    {
                        Vector3 delta = offsetLeftHandlePosition.vector3Value - offsetPosition.vector3Value;
                        offsetRightHandlePosition.vector3Value = offsetPosition.vector3Value - delta;
                    }

                    Vector3 priorOffsetRightHandlePosition = offsetRightHandlePosition.vector3Value;
                    Vector3 newRightHandlePosition = Handles.FreeMoveHandle(
                                                         rightHandlePosition.vector3Value + offsetRightHandlePosition.vector3Value + _globalOffset,
                                                         Quaternion.identity,
                                                         HandleUtility.GetHandleSize(rightHandlePosition.vector3Value) * .1f,
                                                         Vector3.one,
                                                         Handles.DotCap);
                    offsetRightHandlePosition.vector3Value = newRightHandlePosition - rightHandlePosition.vector3Value - _globalOffset;
                    if (priorOffsetRightHandlePosition != offsetRightHandlePosition.vector3Value)
                    {
                        Vector3 delta = offsetPosition.vector3Value - offsetRightHandlePosition.vector3Value;
                        offsetLeftHandlePosition.vector3Value = offsetPosition.vector3Value + delta;
                    }

                    offsetLeftHandlePosition.vector3Value += offsetDeltaPosition;
                    offsetRightHandlePosition.vector3Value += offsetDeltaPosition;
                }
            }

            _primitive.DrawEditorLine(4, Color.black);
            _duplicatePrimitive.DrawEditorLine(8, Color.white);

            SerializedBuilderNode.ApplyModifiedProperties();
        }
#endif
    }
}