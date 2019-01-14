﻿using System;

namespace GeoTetra.GTLogicGraph.Slots
{
    [Serializable]
    public class VertexListPortDescription : PortDescription
    {
        public override PortValueType ValueType { get { return PortValueType.VertexList; } }

        public VertexListPortDescription(NodeEditor owner, string memberName, string displayName, PortDirection portDirection) 
            : base(owner, memberName, displayName, portDirection)
        {
        }

        public override bool IsCompatibleWithInputSlotType(PortValueType inputType)
        {
            return inputType == PortValueType.VertexList; //TODO should this change to storing string?
        }
    }
}
