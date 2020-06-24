using System;

namespace DiscomonProject
{
    public class RockFall : BasicMove
    {
        public override string Name { get; } = "Rock Fall";
        public override string Description { get; } = "The user causes rocks to fall on top of their enemies. Has to chance to lower their speed.";
        public override BasicType Type { get; } = new RockType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 70;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 20;
        
        public RockFall() :base()
        {

        }

        public RockFall(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);
            int dmg = 0;

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
                dmg = ApplyPower(inst, owner);
                enemy.TakeDamage(dmg);
                if(RandomGen.PercentChance(20.0))
                {
                    (double mod, string mess) = enemy.ChangeSpdStage(-1);
                    Result.StatChangeMessages.Add(mess);
                }
            }
            return Result;
        }
    }
}