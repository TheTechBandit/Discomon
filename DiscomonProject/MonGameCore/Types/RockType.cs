using System.Collections.Generic;

namespace DiscomonProject.MonGameCore.Types
{
    public class RockType : BasicType
    {
        public override string Type { get; } = "Rock";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public RockType()
        {

        }

        public RockType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>
            {
                new FireType(),
                new AirType(),
                new ElectricType()
            };
            Disadvantages = new List<BasicType>
            {
                new WaterType(),
                new NatureType(),
                new MetalType(),
                new RockType()
            };
            Immunities = new List<BasicType>();
        }

    }
}