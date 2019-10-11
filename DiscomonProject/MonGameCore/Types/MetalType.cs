using System.Collections.Generic;

namespace DiscomonProject.MonGameCore.Types
{
    public class MetalType : BasicType
    {
        public override string Type { get; } = "Metal";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public MetalType()
        {

        }

        public MetalType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>
            {
                new FeyType(),
                new AirType(),
                new RockType()
            };
            Disadvantages = new List<BasicType>
            {
                new FireType(),
                new ElectricType(),
                new MetalType(),
                new PsychicType()
            };
            Immunities = new List<BasicType>();
        }

    }
}