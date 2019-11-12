using System;

namespace DiscomonProject
{
    public class Poke : BasicMove
    {
        public override string Name { get; } = "Poke";
        public override string Description { get; } = "The user pokes their enemy, lowering their defense by one stage.";
        public override BasicType Type { get; } = new BeastType(true);
        public override bool Contact { get; } = true;
        public override int MaxPP { get; } = 40;
        public override int Power { get; } = 20;
        public override int Accuracy { get; } = 100;
        
        public Poke() :base()
        {

        }

        public Poke(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);
            int dmg = 0;

            //Fail logic
            if(enemy.Fainted || enemy == null || owner.Fainted /*|| enemy.Flying*/)
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
                (double mod, string mess) = enemy.ChangeDefStage(-1);
                Result.EnemyStatChanges[1] = -1;
                Result.StatChangeMessages.Add(mess);
            }
            return Result;
        }
    }
}