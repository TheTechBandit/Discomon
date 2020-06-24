using System;

namespace DiscomonProject
{
    public class DiveBomb : BasicMove
    {
        public override string Name { get; } = "Dive Bomb";
        public override string Description { get; } = "The user flies high into the sky, remaining for one turn before crashing back down, dealing damage.";
        public override BasicType Type { get; } = new AirType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 90;
        public override int Accuracy { get; } = 95;
        public override int MaxPP { get; } = 15;
        
        public DiveBomb() :base()
        {

        }

        public DiveBomb(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            if(!Buffered)
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
                    owner.BufferedMove = this;
                    owner.Status.Flying = true;
                    Buffered = true;
                    Result.Messages.Add($"{owner.Nickname} flew up high!");
                }
                return Result;
            }
            else
                return ApplyBufferedMove(inst, owner);
        }

        public override MoveResult ApplyBufferedMove(CombatInstance inst, BasicMon owner)
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
            //Hit logic
            else
            {
                CurrentPP--;
                dmg = ApplyPower(inst, owner);
                enemy.TakeDamage(dmg);
                owner.BufferedMove = null;
                owner.Status.Flying = false;
                Buffered = false;
            }
            return Result;
        }
    }
}