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
        public event Action<IInputComponent> Changed;

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

        public void HookUpChangedEvent(GraphInput input)
        {
            
        }

        private bool IsEventHandlerRegistered(Delegate prospectiveHandler)
        {
            if (Changed != null)
            {
                foreach (Delegate existingHandler in Changed.GetInvocationList())
                {
                    if (existingHandler == prospectiveHandler)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void SetCurvePrimitive(ObjectEvent value)
        {
            Debug.Log("SetCurvePrimitive");
            _curvePrimitive = value.ObjectValue as CurvePrimitive;
        }
    }
}