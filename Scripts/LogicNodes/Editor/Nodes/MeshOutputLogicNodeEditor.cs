using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Output", "Mesh")]
    [NodeEditorType(typeof(MeshOutputLogicNode))]
    public class MeshOutputLogicNodeEditor : LogicNodeEditor, IOutputNode
    {        
        public override void ConstructNode()
        {
            AddPort(new MeshPortDescription(this, "MeshInput", "In", PortDirection.Input));
        }
    }
}

