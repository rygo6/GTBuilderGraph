using System.Collections;
using System.Collections.Generic;
using GeoTetra.Common.UnityEvents;
using GeoTetra.GTLogicGraph;
using UnityEngine;

namespace GeoTetra.GTBuilder
{
    public class LogicGraphMeshOutput : MonoBehaviour
    {
        [SerializeField]
        private MeshUnityEvent _meshChange;
        
        public void OnMeshChange(ObjectEvent value)
        {
            Mesh mesh = value.ObjectValue as Mesh;
            _meshChange.Invoke(mesh);
        }
    }
}