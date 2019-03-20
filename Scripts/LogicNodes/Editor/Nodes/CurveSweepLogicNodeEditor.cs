using GeoTetra.GTBuilder;
using GeoTetra.GTBuilder.Nodes;
using GeoTetra.GTCommon;
using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Ports;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Curve", "Sweep")]
    [NodeEditorType(typeof(CurveSweepLogicNode))]
    public class CurveSweepLogicNodeEditor : AbstractLogicNodeEditor
    {        
        [SerializeField]
        private bool _flipNormals;

        [SerializeField]
        private Center _center;

        [SerializeField]
        private Bool2 _compressUV;

        [SerializeField]
        private Bool2 _compressUV2;

        private static readonly string[] LabelsFlip = {"on"};
        
        private static readonly string[] LabelsUV = {"U", "V"};
        
        [EnumControl("Rail Center")]
        public Center RailCenter
        {
            get => _center;
            set
            {
                _center = value;
                SetDirty();
            }
        }

        public override void ConstructNode()
        {
            AddSlot(new VertexListSlot(this, "PathVerticesInput", "In Path", SlotDirection.Input));
            AddSlot(new VertexListSlot(this, "RailVerticesInput", "In Rail", SlotDirection.Input));
            
            AddSlot(new BooleanSlot(this, 
                "FlipNormals", 
                "Flip Normals", 
                SlotDirection.Input,
                LabelsFlip,
                () =>  new Bool4(_flipNormals, false, false, false),
                (v) => _flipNormals = v.X));
            
            AddSlot(new BooleanSlot(this, 
                "CompressUV", 
                "Compress UV", 
                SlotDirection.Input,
                LabelsUV,
                () =>  new Bool4(_compressUV.X, _compressUV.Y, false, false),
                (v) => _compressUV = v));
            
            AddSlot(new BooleanSlot(this, 
                "CompressUV2", 
                "Compress UV2", 
                SlotDirection.Input,
                LabelsUV,
                () =>  new Bool4(_compressUV2.X, _compressUV2.Y, false, false),
                (v) => _compressUV2 = v));
            
            AddSlot(new MeshSlot(this, "MeshOutput", "Out", SlotDirection.Output));
        }
    }
}

