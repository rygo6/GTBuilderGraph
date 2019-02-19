using GeoTetra.GTBuilder.Nodes;
using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Curve", "Convert")]
    [NodeEditorType(typeof(CurveConvertLogicNode))]
    public class CurveConvertLogicNodeEditor : AbstractLogicNodeEditor
    {
        [SerializeField] private float _spacing = 1;

        private static readonly string[] Labels = {"X"};

        public override void ConstructNode()
        {
            AddSlot(new CurvePrimitiveLogicSlot(this, "CurvePrimitiveInput", "In", SlotDirection.Input));
            AddSlot(new CurvePrimitiveLogicSlot(this, "UpCurvePrimitiveInput", "Up In", SlotDirection.Input));
            AddSlot(new VertexListSlot(this, "VertexListOutput", "Out", SlotDirection.Output));
            AddSlot(new VectorLogicSlot(
                this,
                "Spacing",
                "Spacing",
                SlotDirection.Input,
                Labels,
                () => new Vector4(_spacing, 0, 0, 0),
                (newValue) => _spacing = newValue.x));
        }
    }
}