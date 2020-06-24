using System;

namespace DiscomonProject
{
    public class ChargeUp : BasicMove
    {
        public override string Name { get; } = "Charge Up";
        public override string Description { get; } = "The user charges up, doubling the power of it's next electric type move.";
        public override BasicType Type { get; } = new ElectricType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 10;
        
        public ChargeUp() :base()
        {

        }

        public ChargeUp(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

            //Fail logic
            if(DefaultFailLogic(enemy, owner) || owner.Status.Charged)
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
                owner.Status.Charged = true;
                Result.Messages.Add($"{owner.Nickname} charges up!");
            }
            return Result;
        }
    }
}