using System;

namespace DiscomonProject
{
    public class Forge : BasicMove
    {
        public override string Name { get; } = "Forge";
        public override string Description { get; } = "The user covers itself in molten metal, curing itself of any status conditions and increasing its defense.";
        public override BasicType Type { get; } = new MetalType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 5;
        
        public Forge() :base()
        {

        }

        public Forge(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

            //Fail logic
            if(SelfMoveFailLogic(owner))
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
                owner.Status.StatusCure();

                (double mod, string mess) = owner.ChangeDefStage(2);
                Result.StatChangeMessages.Add(mess);
            }
            return Result;
        }
    }
}