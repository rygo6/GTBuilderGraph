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
    public class CurveOutputLogicNode : LogicNode
    {
        [Output]
        public event Action<CurvePrimitive> CurvePrimitiveOutput;
       
        [LogicNodePort]
        public void CurvePrimitiveInput(CurvePrimitive value)
        {
//            Debug.Log("CurvePrimitiveOutput CurvePrimitiveInput " + value);
            if (CurvePrimitiveOutput != null) CurvePrimitiveOutput(value);
        }
    }
}