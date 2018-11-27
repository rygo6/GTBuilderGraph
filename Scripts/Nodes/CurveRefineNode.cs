using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Events;

namespace GeoTetra.GTBuilder.Nodes
{
    public class CurveRefineNode : BuilderNode
    {
#if UNITY_EDITOR
        [SerializeField]
        private float _angleThreshold;

        [SerializeField]
        private VertexListEvent _curveGeneratedEvent = new VertexListEvent();

        [HideInInspector]
        [SerializeField]
        private List<Vertex> _curveVertices;

        private List<Vertex> _refinedCurveVertices = new List<Vertex>();

        public override void OnChangeOccured()
        {
            RefineCurve();
            if (_curveGeneratedEvent != null)
            {
                _curveGeneratedEvent.Invoke(_refinedCurveVertices);
            }
        }

        public void Process(List<Vertex> curveVertices)
        {
            _curveVertices = curveVertices;
            RefineCurve();
            if (_curveGeneratedEvent != null)
            {
                _curveGeneratedEvent.Invoke(_refinedCurveVertices);
            }
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
        }
#endif
    }
}