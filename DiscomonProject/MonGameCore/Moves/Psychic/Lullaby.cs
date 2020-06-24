using System;

namespace DiscomonProject
{
    public class Lullaby : BasicMove
    {
        public override string Name { get; } = "Lullaby";
        public override string Description { get; } = "The user sings a soothing melody, making the target fall asleep after one turn.";
        public override BasicType Type { get; } = new PsychicType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 10;
        
        public Lullaby() :base()
        {

        }

        public Lullaby(bool newmove) :base(newmove)
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
                enemy.Status.Sleepy = false;
            }
            return Result;
        }
    }
}