using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Slots;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Output", "Mesh")]
    [NodeEditorType(typeof(MeshOutputLogicNode))]
    public class MeshOutputNodeEditor : NodeEditor, IOutputNode
    {        
        public override void ConstructNode()
        {
            AddSlot(new MeshPortDescription(this, "MeshInput", "In", PortDirection.Input));
        }
    }
}

