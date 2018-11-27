using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GeoTetra.GTBuilder
{
    [System.Serializable]
    public class VertexGroup
    {
        [SerializeField]
        private List<Vertex> _vertices;

        public List<Vertex> Vertices
        {
            get
            {
                return _vertices;
            }
        }

        public VertexGroup()
        {
            _vertices = new List<Vertex>();
        }

        public VertexGroup(List<Vertex> vertices)
        {
            _vertices = vertices;
        }
    }
}