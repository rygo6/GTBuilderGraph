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
        [LogicNodePort]
        public event Action<CurvePrimitive> CurvePrimitiveOutput;
       
        [CurveInput]
        public void CurveInput(Curve value)
        {
            Debug.Log("Vector1InputLogicNode Vector1Input " + value + " " + this.NodeGuid);
            if (CurvePrimitiveOutput != null && value != null) CurvePrimitiveOutput(value.Primitive);
        }
    }
}