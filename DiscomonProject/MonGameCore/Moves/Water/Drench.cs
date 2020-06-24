using System;

namespace DiscomonProject
{
    public class Drench : BasicMove
    {
        public override string Name { get; } = "Drench";
        public override string Description { get; } = "The user drenches the enemy mon, changing its type to water.";
        public override BasicType Type { get; } = new FeyType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 20;
        
        public Drench() :base()
        {

        }

        public Drench(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

            //Fail logic
            if(DefaultFailLogic(enemy, owner) || enemy.HasType("Water"))
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
                enemy.OverrideType = true;
                enemy.OverrideTyping.Add(new WaterType(true));
                Result.Messages.Add($"{enemy.Nickname} is now a **Water** type!");
            }
            return Result;
        }
    }
}