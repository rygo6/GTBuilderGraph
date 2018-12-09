using System;
using System.Collections;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Gizmo;
using GeoTetra.GTLogicGraph;
using UnityEngine;

namespace GeoTetra.GTBuilder.Component
{
    [ExecuteInEditMode]
    public class Curve : MonoBehaviour
    {
        public event Action<MonoBehaviour> Changed;
        
        [SerializeField] private CurvePrimitive _curvePrimitive;

        public CurvePrimitive Primitive
        {
            get { return _curvePrimitive; }
        }

        public void OnChange()
        {
            Debug.Log("OnChange Curve");
            if (Changed != null) Changed(this);
        }
        
        public void SetCurvePrimitive(ObjectEvent value)
        {
            Debug.Log("SetCurvePrimitive");
            _curvePrimitive = value.ObjectValue as CurvePrimitive;
        }
        
        private void OnRenderObject()
        {
//            GizmoSelection.Instance.RenderGizmos();
        }
    }
}