using System;

namespace DiscomonProject
{
    public class HydroJet : BasicMove
    {
        public override string Name { get; } = "Hydro Jet";
        public override string Description { get; } = "The user blasts an opponent with a massive jet of water, dealing immense damage.";
        public override BasicType Type { get; } = new WaterType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 120;
        public override int Accuracy { get; } = 80;
        public override int MaxPP { get; } = 5;
        
        public HydroJet() :base()
        {

        }

        public HydroJet(bool newmove) :base(newmove)
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