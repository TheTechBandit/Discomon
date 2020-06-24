using System;

namespace DiscomonProject
{
    public class Gear : BasicMove
    {
        public override string Name { get; } = "Gear";
        public override string Description { get; } = "The user adjusts the target, lowering their defense while bolstering their attack.";
        public override BasicType Type { get; } = new MetalType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 20;
        
        public Gear() :base()
        {

        }

        public Gear(bool newmove) :base(newmove)
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
                (double mod, string mess) = enemy.ChangeDefStage(-2);
                Result.StatChangeMessages.Add(mess);

                (double mod1, string mess1) = enemy.ChangeAttStage(2);
                Result.StatChangeMessages.Add(mess1);
            }
            return Result;
        }
    }
}