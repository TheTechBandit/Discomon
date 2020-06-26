using System;

namespace DiscomonProject
{
    public class Sunshine : BasicMove
    {
        public override string Name { get; } = "Sunshine";
        public override string Description { get; } = "The sun shines more brightly, increasing the power of nature type attacks.";
        public override BasicType Type { get; } = new NatureType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 10;
        
        public Sunshine() :base()
        {

        }

        public Sunshine(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

            //Fail logic
            if(DefaultFailLogic(enemy, owner))
            {
                Result.Fail = true;
                Result.Hit = false;
            }
            //Miss Logic
            else if(!ApplyAccuracy(inst, owner))
            {
                Result.Miss = true;
                Result.Hit = false;
            }
            //Hit logic
            else
            {
                CurrentPP--;
                Result.Messages.Add(inst.Environment.AttemptSunrise());
            }
            return Result;
        }
    }
}