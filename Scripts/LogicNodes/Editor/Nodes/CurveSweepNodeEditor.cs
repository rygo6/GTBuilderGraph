using GeoTetra.GTBuilder;
using GeoTetra.GTBuilder.Nodes;
using GeoTetra.GTLogicGraph;
using GeoTetra.GTLogicGraph.Slots;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    [Title("Curve", "Sweep")]
    [NodeEditorType(typeof(CurveSweepLogicNode))]
    public class CurveSweepNodeEditor : NodeEditor
    {        
        [SerializeField]
        private bool _flipNormals;

        [SerializeField]
        private Center _center;

        [SerializeField]
        private Bool2 _compressUV;

        [SerializeField]
        private Bool2 _compressUV2;
        
        [NodeToggleControl("Flip Normals")]
        public bool FlipNormals
        {
            get => _flipNormals;
            set
            {
                _flipNormals = value;
                SetDirty();
            }
        }
        
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

        [NodeToggleControl("Compress U")]
        public bool CompressU
        {
            get => _compressUV.X;
            set
            {
                _compressUV.X = value;
                SetDirty();
            }
        }
        
        [NodeToggleControl("Compress V")]
        public bool CompressV
        {
            get => _compressUV.Y;
            set
            {
                _compressUV.Y = value;
                SetDirty();
            }
        }
        
        [NodeToggleControl("Compress U2")]
        public bool CompressU2
        {
            get => _compressUV2.X;
            set
            {
                _compressUV2.X = value;
                SetDirty();
            }
        }
        
        [NodeToggleControl("Compress V2")]
        public bool CompressV2
        {
            get => _compressUV2.Y;
            set
            {
                _compressUV2.Y = value;
                SetDirty();
            }
        }
        
        public override void ConstructNode()
        {
            AddSlot(new VertexListPortDescription(this, "PathVerticesInput", "In Path", PortDirection.Input));
            AddSlot(new VertexListPortDescription(this, "RailVerticesInput", "In Rail", PortDirection.Input));
            AddSlot(new MeshPortDescription(this, "MeshOutput", "Out", PortDirection.Output));
        }
    }
}

