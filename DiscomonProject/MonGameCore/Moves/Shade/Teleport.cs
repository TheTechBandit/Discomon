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
        
        public Teleport() :base()
        {

        }

        public Teleport(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);
            var player = inst.GetPlayer(owner);
            var chara = player.Char;

            //Fail logic
            if(DefaultFailLogic(enemy, owner) || chara.LivingPartyNum() <= 1)
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
                var mon = chara.FirstUsableMon(new List<BasicMon> {owner});
                Result.Swapout = mon;
                Result.Messages.Add($"**{owner.Nickname}** teleports away and {player.Mention} sends out **{mon.Nickname}**!");
            }
            return Result;
        }
    }
}