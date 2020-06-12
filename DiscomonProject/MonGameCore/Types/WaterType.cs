using System.Collections.Generic;

namespace DiscomonProject
{
    public class WaterType : BasicType
    {
        public override string Type { get; } = "Water";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public WaterType() :base()
        {

        }

        public WaterType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>()
            {
                new FireType(),
                new AirType(),
                new RockType()
            };
            Disadvantages = new List<BasicType>()
            {
                new WaterType(),
                new NatureType(),
                new ElectricType(),
                new PrimalType()
            };
            Immunities = new List<BasicType>()
            {
                
            };
        }

    }
}