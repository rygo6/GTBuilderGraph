using System.Collections;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Gizmo;
using UnityEngine;

namespace GeoTetra.GTBuilder.Component
{
    [ExecuteInEditMode]
    public class Curve : MonoBehaviour
    {
        [SerializeField] private CurvePrimitive _curvePrimitive;

        public CurvePrimitive Primitive
        {
            get { return _curvePrimitive; }
        }

        private void OnRenderObject()
        {
            GizmoSelection.Instance.RenderGizmos();
        }
    }
}