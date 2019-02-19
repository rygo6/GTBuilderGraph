using System;

namespace GeoTetra.GTLogicGraph.Ports
{
    [Serializable]
    public class MeshSlot : LogicSlot
    {
        public override SlotValueType ValueType { get { return SlotValueType.Mesh; } }

        public MeshSlot(AbstractLogicNodeEditor owner, string memberName, string displayName, SlotDirection direction) 
            : base(owner, memberName, displayName, direction)
        {
        }

        public override bool IsCompatibleWithInputSlotType(SlotValueType inputType)
        {
            return inputType == SlotValueType.Mesh; //TODO should this change to storing string?
        }
    }
}
