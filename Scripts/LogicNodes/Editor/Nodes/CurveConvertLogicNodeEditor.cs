using GeoTetra.GTBuilder.Nodes;
using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Curve", "Convert")]
    [NodeEditorType(typeof(CurveConvertLogicNode))]
    public class CurveConvertLogicNodeEditor : LogicNodeEditor
    {
        [SerializeField] private float _spacing = 1;

        static readonly string[] Labels = {"X"};

        public override void ConstructNode()
        {
            AddPort(new CurvePrimitivePortDescription(this, "CurvePrimitiveInput", "In", PortDirection.Input));
            AddPort(new CurvePrimitivePortDescription(this, "UpCurvePrimitiveInput", "Up In", PortDirection.Input));
            AddPort(new VertexListPortDescription(this, "VertexListOutput", "Out", PortDirection.Output));
            AddPort(new VectorPortDescription(
                this,
                "Spacing",
                "Spacing",
                PortDirection.Input,
                Labels,
                () => new Vector4(_spacing, 0, 0, 0),
                (newValue) => _spacing = newValue.x));
        }
    }
}