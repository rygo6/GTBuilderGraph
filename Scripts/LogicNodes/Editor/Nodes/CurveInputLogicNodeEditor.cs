using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Input", "Curve")]
    [NodeEditorType(typeof(CurveInputLogicNode))]
    public class CurveInputLogicNodeEditor : AbstractLogicNodeEditor, IInputNode
    {        
        public override void ConstructNode()
        {
            AddSlot(new CurvePrimitiveLogicSlot(this, "CurvePrimitiveOutput", "Out", SlotDirection.Output));
        }
    }
}

