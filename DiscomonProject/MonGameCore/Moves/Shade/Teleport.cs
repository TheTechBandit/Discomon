using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Teleport : BasicMove
    {
        public override string Name { get; } = "Teleport";
        public override string Description { get; } = "The user teleports away.";
        public override BasicType Type { get; } = new ShadeType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 20;
        public override string TargetType { get; } = "Self";
        
        public Teleport() :base()
        {

        }

        public Teleport(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            AddResult();
            var player = inst.GetPlayer(owner);
            var chara = player.Char;

            //Fail logic
            if(SelfMoveFailLogic(owner) || !chara.HasUsableMon())
            {
                Result[TargetNum].Fail = true;
                Result[TargetNum].Hit = false;
            }
            //Hit logic
            else
            {
                CurrentPP--;
                var mon = chara.FirstUsableMon(new List<BasicMon> {owner});
                Result[TargetNum].Swapout = mon;
                Result[TargetNum].Messages.Add($"**{owner.Nickname}** teleports away and {player.Mention} sends out **{mon.Nickname}**!");

                foreach(BasicMon m in inst.GetAllMons())
                {
                    if(m.SelectedMove != null)
                    {
                        for(int i = 0; i < m.SelectedMove.Targets.Count; i++)
                        {
                            if(m.SelectedMove.Targets[i] == owner)
                                m.SelectedMove.Targets[i] = mon;
                        }
                        for(int i = 0; i < m.SelectedMove.ValidTargets.Count; i++)
                        {
                            if(m.SelectedMove.ValidTargets[i] == owner)
                                m.SelectedMove.ValidTargets[i] = mon;
                        }
                    }
                }
            }
            
            return Result;
        }
    }
}