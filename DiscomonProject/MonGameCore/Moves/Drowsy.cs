using System;

namespace DiscomonProject
{
    public class Drowsy : BasicMove
    {
        public override string Name { get; } = "Drowsy";
        public override string Description { get; } = "The user becomes drowsy and falls asleep, healing its HP to full and removing status conditions.";
        public override BasicType Type { get; } = new BeastType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 10;
        
        public Drowsy() :base()
        {

        }

        public Drowsy(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

            //Fail logic
            if(SelfMoveFailLogic(owner))
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
                owner.Status.CureAll();
                Result.StatusMessages.Add(owner.SetAsleep(2));
                owner.CurrentHP = owner.TotalHP;
            }
            return Result;
        }
    }
}