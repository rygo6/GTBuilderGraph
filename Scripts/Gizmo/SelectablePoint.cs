using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GeoTetra.GTBuilder.Gizmo
{
    public class SeralizedSelectablePoint
    {
#if UNITY_EDITOR
        public readonly Color PointColor;
        private readonly SerializedProperty _serializeProperty;
        private readonly Action<SeralizedSelectablePoint> _moved;
        
        public bool Selected { get; set; }
        public Vector3 DeltaPosition { get; private set; }

        public Vector3 GetPosition()
        {
            return _serializeProperty.vector3Value;
        }

        public void Translate(Vector3 translation)
        {
            if (translation.sqrMagnitude > 0)
            {
                DeltaPosition = translation;
                _serializeProperty.vector3Value += translation;
                _moved(this);
            }
        }

        public void SetPosition(Vector3 position)
        {
            DeltaPosition = position - _serializeProperty.vector3Value;
            _serializeProperty.vector3Value = position;
            _moved(this);        
        }

        public SeralizedSelectablePoint(SerializedProperty serializedProperty, Color color, Action<SeralizedSelectablePoint> moved)
        {
            PointColor = color;
            Selected = false;
            _serializeProperty = serializedProperty;
            _moved = moved;
        }

//        public void SetSelectedAfterRangeTest(float range)
//        {
//            Selected = PointInRange(range);
//        }
//
//        public bool PointInRange(float range)
//        {
//			return PointInRange(SceneView.lastActiveSceneView.camera, Event.current.mousePosition * EditorGUIUtility.pixelsPerPoint, range);
//        }
//
//        public bool PointInRange(Camera camera, Vector3 inputPosition, float range)
//        {
//            bool returnValue = false;
//            Vector3 screenPosition = camera.WorldToScreenPoint(GetPosition());
//            screenPosition.y = camera.pixelHeight - screenPosition.y;
//            float distance = Vector2.Distance(inputPosition, screenPosition);
//            if (distance < range)
//            {
//                returnValue = true;
//            }
//            return returnValue;
//        }

        public float DistanceToPointFromMouse()
        {
            Vector3 inputPosition = Event.current.mousePosition * EditorGUIUtility.pixelsPerPoint;
            Camera camera = SceneView.lastActiveSceneView.camera;
            Vector3 screenPosition = camera.WorldToScreenPoint(GetPosition());
            screenPosition.y = camera.pixelHeight - screenPosition.y;
            return Vector2.Distance(inputPosition, screenPosition);
        }
#endif
    }
}