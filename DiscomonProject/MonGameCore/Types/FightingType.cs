using System.Collections.Generic;

namespace DiscomonProject
{
    public class FightingType : BasicType
    {
        public override string Type { get; } = "Fighting";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public FightingType() :base()
        {

        }

        public FightingType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>()
            {
                new BeastType(),
                new RockType(),
                new ColdType()
            };
            Disadvantages = new List<BasicType>()
            {
                new AirType(),
                new MetalType(),
                new PsychicType()
            };
            Immunities = new List<BasicType>()
            {
                new ShadeType()
            };
        }

    }
}