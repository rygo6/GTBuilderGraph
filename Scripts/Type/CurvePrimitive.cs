using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GeoTetra.GTBuilder
{
    [System.Serializable]
    public class CurvePrimitive
    {
        [SerializeField] private bool _closed;

        [SerializeField] private CurveFunction _function;

        [SerializeField] private List<CurveHandle> _handles = new List<CurveHandle>();

        public bool Closed { get { return _closed; } set { _closed = value; } }
        public CurveFunction Function { get { return _function; } set { _function = value; } }
        public List<CurveHandle> Handles { get { return _handles; } }

        public void DrawEditorLine(float width, Color color)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = color;
            int startIndex;
            int endIndex;
            int length = _closed ? _handles.Count : _handles.Count - 1;
            for (int i = 0; i < length; ++i)
            {
                startIndex = i;
                endIndex = i + 1;
                if (endIndex == _handles.Count)
                {
                    endIndex = 0;
                }
                if (_function == CurveFunction.CubicBezier)
                {
                    UnityEditor.Handles.DrawBezier(
                        _handles[startIndex].Position,
                        _handles[endIndex].Position,
                        _handles[startIndex].LeftHandlePosition,
                        _handles[endIndex].RightHandlePosition,
                        color,
                        null,
                        width);
                }
                else if (_function == CurveFunction.Linear)
                {
                    UnityEditor.Handles.DrawAAPolyLine(
                        null,
                        width,
                        new Vector3[] {
                        _handles[startIndex].Position,
                        _handles[endIndex].Position
                        });
                }
            }
#endif
        }
    }
}