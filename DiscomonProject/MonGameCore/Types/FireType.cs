using System.Collections.Generic;

namespace DiscomonProject.MonGameCore.Types
{
    public class FireType : BasicType
    {
        public override string Type { get; } = "Fire";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public FireType()
        {

        }

        public FireType(bool newtype) : base(newtype)
        {
            Advantages = new List<BasicType>
            {
                new NatureType(),
                new BeastType(),
                new MetalType(),
                new ColdType()
            };
            Disadvantages = new List<BasicType>
            {
                new FireType(),
                new WaterType(),
                new AirType(),
                new RockType()
            };
            Immunities = new List<BasicType>();
        }

    }
}