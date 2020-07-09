using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Tackle : BasicMove
    {
        public override string Name { get; } = "Tackle";
        public override string Description { get; } = "The user tackles their enemy, dealing damage.";
        public override BasicType Type { get; } = new BeastType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 40;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 35;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Tackle() :base()
        {

        }

        public Tackle(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            Console.WriteLine($"TA");

            foreach(BasicMon t in targets)
            {
                Console.WriteLine($"TB");
                int dmg = 0;
                Console.WriteLine($"TC");
                AddResult();
                Console.WriteLine($"TD");

                //Fail logic
                if(DefaultFailLogic(t, owner))
                {
                    Console.WriteLine($"TE");
                    Result[TargetNum].Fail = true;
                    Console.WriteLine($"TF");
                    Result[TargetNum].Hit = false;
                    Console.WriteLine($"TG");
                }
                //Miss Logic
                else if(!ApplyAccuracy(inst, owner, t))
                {
                    Console.WriteLine($"TH");
                    Result[TargetNum].Miss = true;
                    Console.WriteLine($"TI");
                    Result[TargetNum].Hit = false;
                    Console.WriteLine($"TJ");
                }
                //Hit Logic
                else
                {
                    Console.WriteLine($"TK");
                    CurrentPP--;
                    Console.WriteLine($"TL");
                    dmg = ApplyPower(inst, owner, t);
                    Console.WriteLine($"TM");
                    t.TakeDamage(dmg);
                    Console.WriteLine($"TN");
                }
                Console.WriteLine($"TO");
            }
            Console.WriteLine($"TP");
            
            return Result;
        }
    }
}