using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GeoTetra.GTBuilder.Nodes
{
    public class CurveGizmoNode : BuilderNode
    {
#if UNITY_EDITOR
        [SerializeField] Color _color;
        [SerializeField] bool _showTicks;

        public List<Vertex> CurveVertices { get; set; }

        public override void OnDrawGizmos()
        {
            if (CurveVertices != null)
            {
                for (int i = 0; i < CurveVertices.Count - 1; ++i)
                {
                    Gizmos.color = _color;
                    Gizmos.DrawLine(CurveVertices[i].Position, CurveVertices[i + 1].Position);
                    if (_showTicks)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawRay(CurveVertices[i].Position, CurveVertices[i].Tangent);
                        Gizmos.color = Color.red;
                        Gizmos.DrawRay(CurveVertices[i].Position, CurveVertices[i].Normal);
                    }
                }
            }
        }
#endif
    }
}