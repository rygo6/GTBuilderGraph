using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Input", "Curve")]
    [NodeEditorType(typeof(CurveInputLogicNode))]
    public class CurveInputLogicNodeEditor : LogicNodeEditor, IInputNode
    {        
        public override void ConstructNode()
        {
            AddPort(new CurvePrimitivePortDescription(this, "CurvePrimitiveOutput", "Out", PortDirection.Output));
        }
    }
}

