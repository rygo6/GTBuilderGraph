using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;

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

