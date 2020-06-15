using System;

namespace DiscomonProject
{
    public class Curl : BasicMove
    {
        public override string Name { get; } = "Curl";
        public override string Description { get; } = "The user curls up, increasing its defense by one stage.";
        public override BasicType Type { get; } = new BeastType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 20;
        
        public Curl() :base()
        {

        }

        public Curl(bool newmove) :base(newmove)
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
                (double mod, string mess) = owner.ChangeDefStage(1);
                Result.StatChangeMessages.Add(mess);
            }
            return Result;
        }
    }
}