using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using GeoTetra.GTBuilder;
using GeoTetra.GTBuilder.Component;
using GeoTetra.GTLogicGraph;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    public class CurveConvertLogicNode : LogicNode
    {
        [SerializeField] private float _spacing = 1;

        [LogicNodePort] public event Action<List<Vertex>> VertexListOutput;

        private CurvePrimitive _primitive;
        private CurvePrimitive _upPrimitive;
        private readonly List<Vertex> _vertices = new List<Vertex>();

        [LogicNodePort]
        public void CurvePrimitiveInput(CurvePrimitive primitive)
        {
            _primitive = primitive;
            Process();
        }

        [LogicNodePort]
        public void UpCurvePrimitiveInput(CurvePrimitive primitive)
        {
            _upPrimitive = primitive;
            Process();
        }

        private void Process()
        {
            if (_primitive.Function == CurveFunction.Linear)
            {
                GenerateLinearCurve();
            }
            else if (_primitive.Function == CurveFunction.CubicBezier)
            {
                GenerateEquiDistanceCurve();
            }

            VertexListOutput?.Invoke(_vertices);
        }

        private void GenerateLinearCurve()
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

        private void GenerateEquiDistanceCurve()
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

                if (_upPrimitive != null && _upPrimitive.Handles.Count == _primitive.Handles.Count)
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
    }
}