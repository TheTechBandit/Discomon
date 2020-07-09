using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class EvasiveManeuvers : BasicMove
    {
        public override string Name { get; } = "Evasive Maneuvers";
        public override string Description { get; } = "The user moves through the air erratically, raising their evasion by one stage.";
        public override BasicType Type { get; } = new AirType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 15;
        public override string TargetType { get; } = "Self";
        
        public EvasiveManeuvers() :base()
        {

        }

        public EvasiveManeuvers(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
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
                (double mod, string mess) = owner.ChangeEvaStage(1);
                Result[TargetNum].StatChangeMessages.Add(mess);
            }

            return Result;
        }
    }
}