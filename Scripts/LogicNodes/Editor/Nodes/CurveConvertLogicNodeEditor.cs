using GeoTetra.GTBuilder.Nodes;
using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Slots;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Curve", "Convert")]
    [NodeEditorType(typeof(CurveConvertLogicNode))]
    public class CurveConvertLogicNodeEditor : LogicNodeEditor
    {        
        [SerializeField] 
        private float _spacing = 1;

        [VectorControlAttribute("Spacing")]
        public float Spacing
        {
            get { return _spacing; }
            set
            {
                _spacing = value;
                SetDirty();
            }
        }
        
        public override void ConstructNode()
        {
            AddPort(new CurvePrimitivePortDescription(this, "CurvePrimitiveInput", "In", PortDirection.Input));
            AddPort(new CurvePrimitivePortDescription(this, "UpCurvePrimitiveInput", "Up In", PortDirection.Input));
            AddPort(new VertexListPortDescription(this, "VertexListOutput", "Out", PortDirection.Output));
        }
    }
}

