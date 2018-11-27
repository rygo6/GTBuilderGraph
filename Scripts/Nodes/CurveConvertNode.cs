using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Events;
using JetBrains.Annotations;
using UnityEditor;

namespace GeoTetra.GTBuilder.Nodes
{
    public class CurveConvertNode : BuilderNode
    {
#if UNITY_EDITOR
        [SerializeField]
        CurvePrimitive _primitive;

        [SerializeField]
        CurvePrimitive _upPrimitive;

        [Range(.1f, 10)]
        [SerializeField]
        float _spacing = 1;

        [SerializeField]
        List<Vertex> _vertices = new List<Vertex>();

        [SerializeField]
        VertexListEvent _processed = new VertexListEvent();

        public CurvePrimitive Primitive
        {
            get { return _primitive; }
            set
            {
                _primitive = value;
                Process();
            }
        }

        public CurvePrimitive UpPrimitive
        {
            get { return _upPrimitive; }
            set
            {
                _upPrimitive = value;
                Process();
            }
        }
        
        public override void OnChangeOccured()
        {
            Process();
        }

        void Process()
        {
            if (_primitive.Function == CurveFunction.Linear)
            {
                GenerateLinearCurve();
            }
            else if (_primitive.Function == CurveFunction.CubicBezier)
            {
                GenerateEquiDistanceCurve();
            }
            if (_processed != null)
            {
                _processed.Invoke(_vertices);
            }
        }

        void GenerateLinearCurve()
        {
            _vertices.Clear();
            for (int i = 0; i < _primitive.Handles.Count; ++i)
            {
                _vertices.Add(new Vertex(_primitive.Handles[i].Position));
            }
            if (_primitive.Closed)
            {
                _vertices.Add(new Vertex(_primitive.Handles[0].Position));
            }
        }

        void GenerateEquiDistanceCurve()
        {
            _vertices.Clear();
            float start = 0f;
            CubicBezierSegment segment;
            CubicBezierSegment upSegment;
            int length = _primitive.Closed ? _primitive.Handles.Count : _primitive.Handles.Count - 1;
            int startIndex;
            int endIndex;
            for (int i = 0; i < length; ++i)
            {
                startIndex = i;
                endIndex = i + 1;
                if (endIndex == _primitive.Handles.Count)
                {
                    endIndex = 0;
                }
                segment = new CubicBezierSegment(
                    _primitive.Handles[startIndex].Position,
                    _primitive.Handles[startIndex].LeftHandlePosition,
                    _primitive.Handles[endIndex].RightHandlePosition,
                    _primitive.Handles[endIndex].Position);

                if (_upPrimitive.Handles.Count == _primitive.Handles.Count)
                {
                    upSegment = new CubicBezierSegment(
                        _upPrimitive.Handles[startIndex].Position,
                        _upPrimitive.Handles[startIndex].LeftHandlePosition,
                        _upPrimitive.Handles[endIndex].RightHandlePosition,
                        _upPrimitive.Handles[endIndex].Position);
                    start = segment.EquiDistancePoints(start, _spacing, _vertices, upSegment, i == length - 1);
                }
                else
                {
                    start = segment.EquiDistancePoints(start, _spacing, _vertices, i == length - 1);
                }
            }
        }
#endif
    }
}