using GeoTetra.GTBuilder.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeoTetra.GTBuilder.Nodes
{
    public class CurveVerticesGroupNode : BuilderNode
    {
#if UNITY_EDITOR
        [SerializeField]
        private int _verticesPerGroup = 20;

        [HideInInspector]
        [SerializeField]
        private List<Vertex> _vertices;

        [SerializeField]
        private VertexGroupListEvent _groupsGenerated = new VertexGroupListEvent();

        private readonly List<VertexGroup> _vertexGroups = new List<VertexGroup>();

        public List<Vertex> PathPoints
        {
            get { return _vertices; }
            set
            {
                _vertices = value;
                Process();
            }
        }

        public override void OnChangeOccured()
        {
            Process();
        }

        private void Process()
        {
            _vertexGroups.Clear();
            VertexGroup vertexGroup = new VertexGroup();
            for (int i = 0; i < _vertices.Count; ++i)
            {
                if (vertexGroup.Vertices.Count == _verticesPerGroup)
                {
                    vertexGroup.Vertices.Add(_vertices[i]);
                    _vertexGroups.Add(vertexGroup);
                    vertexGroup = new VertexGroup();
                }
                vertexGroup.Vertices.Add(_vertices[i]);
            }            
            if (vertexGroup.Vertices.Count > 1)
            {
                _vertexGroups.Add(vertexGroup);
            }
            _groupsGenerated.Invoke(_vertexGroups);
        }

        public override void OnDrawGizmosSelected()
        {
            for (int i = 0; i < _vertexGroups.Count; ++i)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_vertexGroups[i].Vertices[0].Position, 8);
            }
        }
#endif
    }
}