using System;
using GeoTetra.GTLogicGraph;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    public class MeshOutputLogicNode : LogicNode
    {
        [Output]
        public event Action<Mesh> MeshOutput;
       
        [LogicNodePort]
        public void MeshInput(Mesh value)
        {
            Debug.Log("MeshOutputLogicNode MeshInput " + value);
            if (MeshOutput != null) MeshOutput(value);
        }
    }
}