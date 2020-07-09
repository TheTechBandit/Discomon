using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Disarm : BasicMove
    {
        public override string Name { get; } = "Disarm";
        public override string Description { get; } = "The user disables the move the opponent used or will use this turn. Fails if the opponent already has a move disabled.";
        public override BasicType Type { get; } = new PsychicType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 10;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Disarm() :base()
        {

        }

        public Disarm(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            
            foreach(BasicMon t in targets)
            {
                AddResult();

                //Fail logic
                if(DefaultFailLogic(t, owner) || t.HasDisabledMove())
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
                    var move = t.SelectedMove;
                    move.Disabled = true;
                    Result[TargetNum].StatChangeMessages.Add($"{owner.Nickname} disabled {t.Nickname}'s {move.Name}!");
                }
            }
            
            return Result;
        }
    }
}