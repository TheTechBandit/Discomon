using System;
using System.Collections.Generic;

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
        public override string TargetType { get; } = "SingleEnemy";
        
        public DiveBomb() :base()
        {

        }

        public DiveBomb(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            if(!Buffered)
            {
                ResetResult();
                AddResult();

                //Fail logic
                if(SelfMoveFailLogic(owner))
                {
                    Result[TargetNum].Fail = true;
                    Result[TargetNum].Hit = false;
                }
                //Hit logic
                else
                {
                    CurrentPP--;
                    owner.BufferedMove = this;
                    owner.Status.Flying = true;
                    Buffered = true;
                    Result[TargetNum].Messages.Add($"{owner.Nickname} flew up high!");
                }
                return Result;
            }
            else
                return ApplyBufferedMove(inst, owner, targets);
        }

        public override List<MoveResult> ApplyBufferedMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();

            foreach(BasicMon t in targets)
            {
                int dmg = 0;
                AddResult();

                //Fail logic
                if(DefaultFailLogic(t, owner))
                {
                    Result[TargetNum].Fail = true;
                    Result[TargetNum].Hit = false;
                }
                //Miss Logic
                else if(!ApplyAccuracy(inst, owner, t))
                {
                    Result[TargetNum].Miss = true;
                    Result[TargetNum].Hit = false;
                }
                //Hit logic
                else
                {
                    CurrentPP--;
                    dmg = ApplyPower(inst, owner, t);
                    t.TakeDamage(dmg);
                }

                owner.BufferedMove = null;
                owner.Status.Flying = false;
                Buffered = false;
            }

            return Result;
        }
    }
}