using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Slots;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Input", "Curve")]
    [NodeEditorType(typeof(CurveInputLogicNode))]
    public class CurveInputNodeEditor : NodeEditor, IInputNode
    {        
        public override void ConstructNode()
        {
            AddSlot(new CurvePrimitivePortDescription(this, "CurvePrimitiveOutput", "Out", PortDirection.Output));
        }
    }
}

