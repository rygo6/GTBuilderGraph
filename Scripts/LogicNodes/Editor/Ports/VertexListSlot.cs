using System;

namespace GeoTetra.GTLogicGraph.Ports
{
    [Serializable]
    public class VertexListSlot : LogicSlot
    {
        public override SlotValueType ValueType { get { return SlotValueType.VertexList; } }

        public VertexListSlot(AbstractLogicNodeEditor owner, string memberName, string displayName, SlotDirection portDirection) 
            : base(owner, memberName, displayName, portDirection)
        {
        }

        public override bool IsCompatibleWithInputSlotType(SlotValueType inputType)
        {
            return inputType == SlotValueType.VertexList; //TODO should this change to storing string?
        }
    }
}
