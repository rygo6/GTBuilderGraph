using GeoTetra.GTBuilder.Nodes;
using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Slots;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Curve", "Duplicate")]
    [NodeEditorType(typeof(CurveDuplicateLogicNode))]
    public class CurveDuplicateLogicNodeEditor : LogicNodeEditor
    {        
        [SerializeField]
        private Vector3 _globalOffset;
        
        [VectorControlAttribute("Global Offset")]
        public Vector3 GlobalOffset
        {
            get { return _globalOffset; }
            set
            {
                _globalOffset = value;
                SetDirty();
            }
        }
        
        public override void ConstructNode()
        {
            AddPort(new CurvePrimitivePortDescription(this, "CurvePrimitiveInput", "In", PortDirection.Input));
            AddPort(new CurvePrimitivePortDescription(this, "CurvePrimitiveOutput", "Out", PortDirection.Output));
        }
    }
}

