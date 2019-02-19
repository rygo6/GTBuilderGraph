using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Output", "Mesh")]
    [NodeEditorType(typeof(MeshOutputLogicNode))]
    public class MeshOutputLogicNodeEditor : AbstractLogicNodeEditor, IOutputNode
    {        
        public override void ConstructNode()
        {
            AddSlot(new MeshSlot(this, "MeshInput", "In", SlotDirection.Input));
        }
    }
}

