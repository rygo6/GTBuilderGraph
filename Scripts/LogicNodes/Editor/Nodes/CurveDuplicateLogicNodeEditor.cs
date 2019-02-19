using GeoTetra.GTBuilder.Nodes;
using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Curve", "Duplicate")]
    [NodeEditorType(typeof(CurveDuplicateLogicNode))]
    public class CurveDuplicateLogicNodeEditor : AbstractLogicNodeEditor
    {        
        [SerializeField]
        private Vector3 _globalOffset;
        
        static readonly string[] Labels = {"X", "Y", "Z"};
        
        public override void ConstructNode()
        {
            AddSlot(new CurvePrimitiveLogicSlot(this, "CurvePrimitiveInput", "In", SlotDirection.Input));
            AddSlot(new VectorLogicSlot(
                this, 
                "GlobalOffset", 
                "Global Offset", 
                SlotDirection.Input,
                Labels,
                () => _globalOffset,
                (newValue) => _globalOffset = newValue));
            AddSlot(new CurvePrimitiveLogicSlot(this, "CurvePrimitiveOutput", "Out", SlotDirection.Output));
        }
    }
}

