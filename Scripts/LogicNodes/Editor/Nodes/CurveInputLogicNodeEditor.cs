using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Slots;

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

