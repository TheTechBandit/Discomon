using System;

namespace DiscomonProject
{
    public class Wish : BasicMove
    {
        public override string Name { get; } = "Wish";
        public override string Description { get; } = "The user heals slightly, cures any status conditions, and raises their own attack and affinity.";
        public override BasicType Type { get; } = new FeyType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 5;
        
        public Wish() :base()
        {

        }

        public Wish(bool newmove) :base(newmove)
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
                owner.Status.StatusCure();
                var amount = owner.TotalHP/10;
                owner.Restore(amount);
                Result.SelfHeal = amount;

                (double mod, string mess) = owner.ChangeAttStage(1);
                Result.StatChangeMessages.Add(mess);

                (double mod1, string mess1) = owner.ChangeAffStage(1);
                Result.StatChangeMessages.Add(mess1);
            }
            return Result;
        }
    }
}