using System;

namespace GeoTetra.GTLogicGraph.Slots
{
    [Serializable]
    public class CurvePrimitivePortDescription : PortDescription
    {
        public override PortValueType ValueType { get { return PortValueType.CurvePrimitive; } }

        public CurvePrimitivePortDescription(NodeEditor owner, string memberName, string displayName, PortDirection portDirection) 
            : base(owner, memberName, displayName, portDirection)
        {
        }

        public override bool IsCompatibleWithInputSlotType(PortValueType inputType)
        {
            return inputType == PortValueType.CurvePrimitive; //TODO should this change to storing string?
        }
    }
}
