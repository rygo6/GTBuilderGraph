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
    public class CurveDuplicateLogicNode : LogicNode
    {
        [SerializeField]
        private Vector3 _globalOffset;
        
        private CurvePrimitive _primitive;

        private readonly CurvePrimitive _duplicatePrimitive = new CurvePrimitive();

        private readonly List<CurveHandle> _offsets = new List<CurveHandle>();
        
        [LogicNodePort]
        public event Action<CurvePrimitive> CurvePrimitiveOutput;
       
        [LogicNodePort]
        public void CurvePrimitiveInput(CurvePrimitive value)
        {
            _primitive = value;
            _duplicatePrimitive.Function = _primitive.Function;
            _duplicatePrimitive.Closed = _primitive.Closed;

            while (_offsets.Count < _primitive.Handles.Count)
            {
                _offsets.Add(new CurveHandle());
            }
            while (_offsets.Count > _primitive.Handles.Count)
            {
                _offsets.RemoveAt(_offsets.Count - 1);
            }

            while (_duplicatePrimitive.Handles.Count < _primitive.Handles.Count)
            {
                _duplicatePrimitive.Handles.Add(new CurveHandle());
            }
            while (_duplicatePrimitive.Handles.Count > _primitive.Handles.Count)
            {
                _duplicatePrimitive.Handles.RemoveAt(_offsets.Count - 1);
            }

            CreateDuplicateCurve();
            
//            Debug.Log("CurvePrimitiveInput CurvePrimitiveInput " + value + " offset " + _globalOffset);
            CurvePrimitiveOutput?.Invoke(_duplicatePrimitive);
        }
        
        void CreateDuplicateCurve()
        {
            for (int i = 0; i < _primitive.Handles.Count; ++i)
            {
                _duplicatePrimitive.Handles[i] = new CurveHandle(
                    _primitive.Handles[i].Position + _offsets[i].Position + _globalOffset,
                    _primitive.Handles[i].RightHandlePosition + _offsets[i].RightHandlePosition + _globalOffset,
                    _primitive.Handles[i].LeftHandlePosition + _offsets[i].LeftHandlePosition + _globalOffset);
            }
        }

    }
}