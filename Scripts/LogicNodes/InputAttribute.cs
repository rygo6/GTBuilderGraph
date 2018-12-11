using System;
using System.Reflection;
using GeoTetra.GTBuilder.Component;
using UnityEngine;
using UnityEngine.UI;

namespace GeoTetra.GTLogicGraph
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CurveInputAttribute : InputAttribute
    {
        public override void HookUpMethodInvoke(LogicNode node, MethodInfo method, GraphInput graphInput)
        {
            graphInput.Validate = () => OnValidate(node, method, graphInput);
        }

        public override Type InputType()
        {
            return typeof(Curve);
        }
        
        private void OnValidate(LogicNode node, MethodInfo method, GraphInput graphInput)
        {
//            if (!Mathf.Approximately(graphInput.FloatValueX, _priorFloatValueX))
//            {
                Curve curve = graphInput.ComponentValue as Curve;
                method.Invoke(node, new object[] {curve});
//                _priorFloatValueX = graphInput.FloatValueX;
//            }
        }
    }
}