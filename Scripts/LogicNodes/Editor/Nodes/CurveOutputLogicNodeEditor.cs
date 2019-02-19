using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Output", "Curve")]
    [NodeEditorType(typeof(CurveOutputLogicNode))]
    public class CurveOutputLogicNodeEditor : AbstractLogicNodeEditor, IOutputNode
    {        
        public override void ConstructNode()
        {
            AddSlot(new CurvePrimitiveLogicSlot(this, "CurvePrimitiveInput", "In", SlotDirection.Input));
        }
    }
}

