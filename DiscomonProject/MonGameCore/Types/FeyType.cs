using System.Collections.Generic;

namespace DiscomonProject
{
    public class FeyType : BasicType
    {
        public override string Type { get; } = "Fey";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public FeyType() :base()
        {

        }

        public FeyType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>()
            {
                new BeastType(),
                new ShadeType()
            };
            Disadvantages = new List<BasicType>()
            {
                new MetalType(),
                new RockType()
            };
            Immunities = new List<BasicType>()
            {
                
            };
        }

    }
}