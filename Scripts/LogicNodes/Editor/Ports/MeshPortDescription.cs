using System;

namespace GeoTetra.GTLogicGraph.Ports
{
    [Serializable]
    public class MeshPortDescription : PortDescription
    {
        public override PortValueType ValueType { get { return PortValueType.Mesh; } }

        public MeshPortDescription(LogicNodeEditor owner, string memberName, string displayName, PortDirection portDirection) 
            : base(owner, memberName, displayName, portDirection)
        {
        }

        public override bool IsCompatibleWithInputSlotType(PortValueType inputType)
        {
            return inputType == PortValueType.Mesh; //TODO should this change to storing string?
        }
    }
}
