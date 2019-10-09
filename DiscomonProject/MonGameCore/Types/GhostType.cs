using System.Collections.Generic;

namespace DiscomonProject
{
    public class GhostType : BasicType
    {
        public override string Type { get; } = "Ghost";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public GhostType() :base()
        {

        }

        public GhostType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>()
            {
                new FireType(),
                new GhostType(),
                new PsychicType()
            };
            Disadvantages = new List<BasicType>()
            {
                new FeyType()
            };
            Immunities = new List<BasicType>()
            {
                new BeastType()
            };
        }

    }
}