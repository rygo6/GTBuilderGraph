using GeoTetra.GTBuilder;
using GeoTetra.GTBuilder.Nodes;
using GeoTetra.GTCommon;
using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Curve", "Refine")]
    [NodeEditorType(typeof(CurveRefineLogicNode))]
    public class CurveRefineLogicNodeEditor : AbstractLogicNodeEditor
    {        
        [SerializeField]
        private float _angleThreshold = 1;
        
        private static readonly string[] Labels = {"X"};

        public override void ConstructNode()
        {
            AddSlot(new VertexListSlot(this, nameof(CurveRefineLogicNode.VertexListInput), "In", SlotDirection.Input));
            AddSlot(new VertexListSlot(this, nameof(CurveRefineLogicNode.VertexListOutput), "Out", SlotDirection.Output));
            AddSlot(new VectorLogicSlot(
                this,
                nameof(CurveRefineLogicNode.AngleThresholdInput),
                "AngleThreshold",
                SlotDirection.Input,
                Labels,
                () => new Vector4(_angleThreshold, 0, 0, 0),
                (newValue) => _angleThreshold = newValue.x));
        }
    }
}