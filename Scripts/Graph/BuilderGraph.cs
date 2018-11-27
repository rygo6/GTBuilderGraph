using GeoTetra.GTBuilder.Gizmo;
using GeoTetra.GTBuilder.Nodes;
using System.Collections.Generic;
using UnityEngine;

namespace GeoTetra.GTBuilder.Graph
{
    [ExecuteInEditMode]
    public class BuilderGraph : MonoBehaviour
    {
//        [HideInInspector]
        [SerializeField]
        private List<BuilderNode> _logicNodes = new List<BuilderNode>();

        public List<BuilderNode> LogicNodes { get { return _logicNodes; } }

#if UNITY_EDITOR
        private Mesh _gizmoMesh;
        private Material _gizmoMaterial;

        public BuilderNode SelectedNode { get; set; }
        public Mesh GizmoMesh { get { return _gizmoMesh; } set { _gizmoMesh = value; } }

        private void OnDrawGizmos()
        {
            foreach (BuilderNode logicNode in _logicNodes)
            {
                logicNode.OnDrawGizmos();
            }
            if (SelectedNode != null)
            {
                SelectedNode.OnDrawGizmosSelected();
            }
        }

        private void OnDestroy()
        {
            if (_gizmoMesh)
                DestroyImmediate(_gizmoMesh);
            if (_gizmoMaterial)
                DestroyImmediate(_gizmoMaterial);
        }

        private void OnRenderObject()
        {
            if (BuilderGraphUtility.SelectedBuilderGraph == this && SelectedNode != null)
            {
                SelectedNode.OnRenderObject();
                GizmoSelection.Instance.RenderGizmos();
            }
        }
        
        
#endif
    }
}