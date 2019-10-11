using System.Collections.Generic;

namespace DiscomonProject.MonGameCore.Types
{
    public class BeastType : BasicType
    {
        public override string Type { get; } = "Beast";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public BeastType()
        {

        }

        public BeastType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>
            {
                new NatureType(),
                new PrimalType()
            };
            Disadvantages = new List<BasicType>
            {
                new FireType(),
                new FeyType(),
                new PsychicType(),
                new SonicType()
            };
            Immunities = new List<BasicType>
            {
                new GhostType()
            };
        }

    }
}