using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Events;

namespace GeoTetra.GTBuilder.Nodes
{
    public class CurveOrientNode : BuilderNode
    {
#if UNITY_EDITOR
        [SerializeField] Vector3 _defaultUp = Vector3.up;
        [SerializeField] VertexListEvent _curveGeneratedEvent = new VertexListEvent();
        [HideInInspector] [SerializeField] List<Vertex> _curveVertices;

        public void Process(List<Vertex> curveVertices)
        {
            _curveVertices = curveVertices;
            for (int i = 0; i < _curveVertices.Count; ++i)
            {
                Vector3 direction = Vector3.zero;
                Vector3 normal = Vector3.up;
                bool directionASet = false;
                if (i + 1 < _curveVertices.Count)
                {
                    direction = _curveVertices[i + 1].Position - _curveVertices[i].Position;
                    directionASet = true;
                }
                if (i > 0)
                {
                    Vector3 directionB = _curveVertices[i].Position - _curveVertices[i - 1].Position;
                    if (directionASet)
                    {
                        direction = (direction + directionB) / 2f;
                        normal = Vector3.Cross(direction, directionB);
                    }
                    else
                    {
                        direction = directionB;
                    } 
                }
                direction.Normalize();
                _curveVertices[i] = new Vertex(_curveVertices[i].Position, normal, direction);
            }
            if (_curveGeneratedEvent != null)
            {
                _curveGeneratedEvent.Invoke(_curveVertices);
            }
        }
#endif
    }
}