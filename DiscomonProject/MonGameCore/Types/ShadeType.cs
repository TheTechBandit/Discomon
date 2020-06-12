using System.Collections.Generic;

namespace DiscomonProject
{
    public class ShadeType : BasicType
    {
        public override string Type { get; } = "Shade";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public ShadeType() :base()
        {

        }

        public ShadeType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>()
            {
                new FireType(),
                new ShadeType(),
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