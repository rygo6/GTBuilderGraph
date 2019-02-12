using GeoTetra.GTBuilder.Nodes;
using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Curve", "Duplicate")]
    [NodeEditorType(typeof(CurveDuplicateLogicNode))]
    public class CurveDuplicateLogicNodeEditor : LogicNodeEditor
    {        
        [SerializeField]
        private Vector3 _globalOffset;
        
        static readonly string[] Labels = {"X", "Y", "Z"};
        
        public override void ConstructNode()
        {
            AddPort(new CurvePrimitivePortDescription(this, "CurvePrimitiveInput", "In", PortDirection.Input));
            AddPort(new VectorPortDescription(
                this, 
                "GlobalOffset", 
                "Global Offset", 
                PortDirection.Input,
                Labels,
                () => _globalOffset,
                (newValue) => _globalOffset = newValue));
            AddPort(new CurvePrimitivePortDescription(this, "CurvePrimitiveOutput", "Out", PortDirection.Output));
        }
    }
}

