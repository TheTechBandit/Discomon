using System.Collections.Generic;

namespace DiscomonProject.MonGameCore.Types
{
    public class SonicType : BasicType
    {
        public override string Type { get; } = "Sonic";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public SonicType()
        {

        }

        public SonicType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>
            {
                new BeastType(),
                new PsychicType(),
                new PrimalType()
            };
            Disadvantages = new List<BasicType>
            {
                new WaterType()
            };
            Immunities = new List<BasicType>();
        }

    }
}