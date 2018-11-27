using UnityEngine;
using System.Collections;

namespace GeoTetra.GTBuilder
{
    [System.Serializable]
    public struct CurveHandle
    {
        public Vector3 Position;
        public Vector3 RightHandlePosition;
        public Vector3 LeftHandlePosition;

        public CurveHandle(Vector3 position, Vector3 rightHandlePosition, Vector3 leftHandlePosition)
        {
            Position = position;
            RightHandlePosition = rightHandlePosition;
            LeftHandlePosition = leftHandlePosition;
        }
    }
}