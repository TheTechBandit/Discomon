using System;

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
        
        public Disarm() :base()
        {

        }

        public Disarm(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

            //Fail logic
            if(DefaultFailLogic(enemy, owner) || enemy.HasDisabledMove())
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
                var move = enemy.SelectedMove;
                move.Disabled = true;
                Result.StatChangeMessages.Add($"{owner.Nickname} disabled {enemy.Nickname}'s {move.Name}!");
            }
            return Result;
        }
    }
}