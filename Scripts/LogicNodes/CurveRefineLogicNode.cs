using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using GeoTetra.GTBuilder.Events;
using GeoTetra.GTLogicGraph;

namespace GeoTetra.GTBuilder
{
    public class CurveRefineLogicNode : LogicNode
    {
        public event Action<List<Vertex>> VertexListOutput;

        [SerializeField]
        private float _angleThreshold;

        private List<Vertex> _curveVertices;

        private readonly List<Vertex> _refinedCurveVertices = new List<Vertex>();

        public void AngleThresholdInput(float value)
        {
            Debug.Log("AngleThresholdInput");
            _angleThreshold = value;
            Process();
        }

        public void VertexListInput(List<Vertex> vertices)
        {
            Debug.Log("VertexListInput");
            _curveVertices = vertices;
            Process();
        }

        private void Process()
        {
            RefineCurve();
            VertexListOutput?.Invoke(_refinedCurveVertices);
        }

        private void RefineCurve()
        {
            _refinedCurveVertices.Clear();
            _refinedCurveVertices.AddRange(_curveVertices);

            int index = 1;
            while (index < _refinedCurveVertices.Count - 1)
            {
                Vector3 directionA = _refinedCurveVertices[index].Position - _refinedCurveVertices[index - 1].Position;
                Vector3 directionB = _refinedCurveVertices[index + 1].Position - _refinedCurveVertices[index].Position;
                if (Vector3.Angle(directionA, directionB) < _angleThreshold)
                {
                    _refinedCurveVertices.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
            Debug.Log($"Refine curve start {_curveVertices.Count} after {_refinedCurveVertices.Count} threshold {_angleThreshold}");
        }
    }
}