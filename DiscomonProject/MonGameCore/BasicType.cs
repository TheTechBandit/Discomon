using System.Collections.Generic;
using System.Linq;

namespace DiscomonProject.MonGameCore
{
    public class BasicType
    {
        public virtual string Type { get; }
        public virtual List<BasicType> Advantages { get; }
        public virtual List<BasicType> Disadvantages { get; }
        public virtual List<BasicType> Immunities { get; }
        public virtual string Description { get; }

        public BasicType()
        {

        }

        public BasicType(bool newtype)
        {

        }

        public double ParseEffectiveness(List<BasicType> def)
        {
            var effect = 1.0;

            var defstr = $"{def[0].Type}";
            if(def.Count > 1)
                defstr += $"/{def[1].Type}";
            System.Console.WriteLine($"{Type} vs. {defstr}");
            System.Console.WriteLine($"{this.GetType().ToString()}");

            foreach(BasicType ty in def)
            {
                foreach (var adv in Advantages.Where(adv => ty.GetType() == adv.GetType()))
                {
                    System.Console.WriteLine($"{Type} is advantagous against {ty.Type}");
                    effect *= 2.0;
                }

                foreach (var dis in Disadvantages.Where(dis => ty.GetType() == dis.GetType()))
                {
                    System.Console.WriteLine($"{Type} is disadvantagous against {ty.Type}");
                    effect *= 0.5;
                }
            }

            return effect;
        }

        public override string ToString() 
            => $"{Type}";
    }
}