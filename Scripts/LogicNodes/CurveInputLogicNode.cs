using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using GeoTetra.GTBuilder;
using GeoTetra.GTBuilder.Component;
using GeoTetra.GTLogicGraph;
using UnityEngine;

namespace GeoTetra.GTBuilderGraph
{
    public class CurveInputLogicNode : LogicNode
    {
        [NodePort]
        public event Action<CurvePrimitive> CurvePrimitiveOutput;
       
        [CurveInput]
        public void CurveInput(Curve value)
        {
            Debug.Log("Vector1InputLogicNode Vector1Input " + value);
            if (CurvePrimitiveOutput != null) CurvePrimitiveOutput(value.Primitive);
        }
    }
}