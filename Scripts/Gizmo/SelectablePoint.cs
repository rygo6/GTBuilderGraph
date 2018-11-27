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
        private readonly SerializedProperty SerializeProperty;
        private readonly Action<SeralizedSelectablePoint> onMoved;
        
        public bool Selected { get; set; }
        public Vector3 DeltaPosition { get; private set; }

        public Vector3 GetPosition()
        {
            return SerializeProperty.vector3Value;
        }

        public void Translate(Vector3 translation)
        {
            DeltaPosition = translation;
            SerializeProperty.vector3Value += translation;
            onMoved(this);
        }

        public void SetPosition(Vector3 position)
        {
            DeltaPosition = position - SerializeProperty.vector3Value;
            SerializeProperty.vector3Value = position;
            onMoved(this);        
        }

        public SeralizedSelectablePoint(SerializedProperty serializedProperty, Color color, Action<SeralizedSelectablePoint> onMoved)
        {
            PointColor = color;
            Selected = false;
            SerializeProperty = serializedProperty;
            this.onMoved = onMoved;
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