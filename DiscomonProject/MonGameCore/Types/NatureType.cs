using System.Collections.Generic;

namespace DiscomonProject
{
    public class NatureType : BasicType
    {
        public override string Type { get; } = "Nature";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public NatureType() :base()
        {

        }

        public NatureType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>()
            {
                new WaterType(),
                new FeyType(),
                new ElectricType(),
                new RockType()
            };
            Disadvantages = new List<BasicType>()
            {
                new FireType(),
                new NatureType(),
                new BeastType(),
                new ColdType()
            };
            Immunities = new List<BasicType>()
            {
                
            };
        }

    }
}