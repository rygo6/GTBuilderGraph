using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Slots;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Output", "Curve")]
    [NodeEditorType(typeof(CurveOutputLogicNode))]
    public class CurveOutputLogicNodeEditor : LogicNodeEditor, IOutputNode
    {        
        public override void ConstructNode()
        {
            AddPort(new CurvePrimitivePortDescription(this, "CurvePrimitiveInput", "In", PortDirection.Input));
        }
    }
}

