using System;
using System.Collections;
using System.Collections.Generic;
using GeoTetra.GTBuilder.Gizmo;
using GeoTetra.GTLogicGraph;
using UnityEngine;

namespace GeoTetra.GTBuilder.Component
{
    [ExecuteInEditMode]
    public class Curve : MonoBehaviour, IInputComponent
    {
        public event Action<IInputComponent, long> Changed;

        [SerializeField] private CurvePrimitive _curvePrimitive;

        public CurvePrimitive Primitive
        {
            get { return _curvePrimitive; }
        }

        public void OnChange()
        {
//            Debug.Log("OnChange Curve");
            Changed?.Invoke(this, System.DateTime.Now.Ticks);
        }

        public void SetCurvePrimitive(ObjectEvent value)
        {
//            Debug.Log("SetCurvePrimitive");
            _curvePrimitive = value.ObjectValue as CurvePrimitive;
        }

        private void OnDrawGizmosSelected()
        {
            Primitive.DrawEditorLine(4, Color.white);
            GizmoSelection.Instance.RenderGizmos();
        }
        
        private void OnDrawGizmos()
        {
            Primitive.DrawEditorLine(2, Color.gray);
        }
    }
}