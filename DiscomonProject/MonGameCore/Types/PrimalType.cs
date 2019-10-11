using System.Collections.Generic;

namespace DiscomonProject.MonGameCore.Types
{
    public class PrimalType : BasicType
    {
        public override string Type { get; } = "Primal";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public PrimalType()
        {

        }

        public PrimalType(bool newtype) :base(newtype)
        {
            Advantages = new List<BasicType>
            {
                new WaterType(),
                new FeyType(),
                new PrimalType()
            };
            Disadvantages = new List<BasicType>
            {
                new BeastType(),
                new AirType(),
                new ColdType(),
                new SonicType()
            };
            Immunities = new List<BasicType>();
        }

    }
}