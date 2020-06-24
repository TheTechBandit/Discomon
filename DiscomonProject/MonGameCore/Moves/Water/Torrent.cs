using System;

namespace DiscomonProject
{
    public class Torrent : BasicMove
    {
        public override string Name { get; } = "Torrent";
        public override string Description { get; } = "The user attacks everything around it with a massive torrent of water, dealing damage.";
        public override BasicType Type { get; } = new WaterType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 90;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 10;
        
        public Torrent() :base()
        {

        }

        public Torrent(bool newmove) :base(newmove)
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
            }
            return Result;
        }
    }
}