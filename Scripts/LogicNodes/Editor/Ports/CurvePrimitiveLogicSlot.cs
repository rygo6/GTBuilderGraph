using System;

namespace GeoTetra.GTLogicGraph.Ports
{
    [Serializable]
    public class CurvePrimitiveLogicSlot : LogicSlot
    {
        public override SlotValueType ValueType { get { return SlotValueType.CurvePrimitive; } }

        public CurvePrimitiveLogicSlot(AbstractLogicNodeEditor owner, string memberName, string displayName, SlotDirection direction) 
            : base(owner, memberName, displayName, direction)
        {
        }

        public override bool IsCompatibleWithInputSlotType(SlotValueType inputType)
        {
            return inputType == SlotValueType.CurvePrimitive; //TODO should this change to storing string?
        }
    }
}
