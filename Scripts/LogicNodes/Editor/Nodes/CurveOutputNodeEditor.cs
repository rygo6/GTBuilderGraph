using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Slots;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Output", "Curve")]
    [NodeEditorType(typeof(CurveOutputLogicNode))]
    public class CurveOutputNodeEditor : NodeEditor, IOutputNode
    {        
        public override void ConstructNode()
        {
            AddSlot(new CurvePrimitivePortDescription(this, "CurvePrimitiveInput", "In", PortDirection.Input));
        }
    }
}

