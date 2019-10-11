using System.Collections.Generic;

namespace DiscomonProject.MonGameCore.Types
{
    public class ElectricType : BasicType
    {
        public override string Type { get; } = "Electric";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public ElectricType()
        {

        }

        public ElectricType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>
            {
                new WaterType(),
                new AirType(),
                new MetalType()
            };
            Disadvantages = new List<BasicType>
            {
                new ElectricType(),
                new RockType()
            };
            Immunities = new List<BasicType>();
        }

    }
}