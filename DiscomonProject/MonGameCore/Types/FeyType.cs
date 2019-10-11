using System.Collections.Generic;

namespace DiscomonProject.MonGameCore.Types
{
    public class FeyType : BasicType
    {
        public override string Type { get; } = "Fey";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public FeyType()
        {

        }

        public FeyType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>
            {
                new BeastType(),
                new GhostType()
            };
            Disadvantages = new List<BasicType>
            {
                new MetalType(),
                new RockType()
            };
            Immunities = new List<BasicType>();
        }

    }
}