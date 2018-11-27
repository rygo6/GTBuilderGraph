using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace GeoTetra.GTBuilder
{
    [System.Serializable]
    public class MeshGroup
    {
        [SerializeField]
        private List<Mesh> _meshes;

        public ReadOnlyCollection<Mesh> Meshes
        {
            get
            {
               return _meshes.AsReadOnly();
            }
        }

        public MeshGroup()
        {
            _meshes = new List<Mesh>();
        }

        public MeshGroup(List<Mesh> meshes)
        {
            _meshes = meshes;
        }

        public void AddMesh(Mesh mesh)
        {
            _meshes.Add(mesh);
        }

        public void Clear()
        {
            _meshes.Clear();
        }
    }
}