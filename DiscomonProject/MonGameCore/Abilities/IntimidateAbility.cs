using System;
using System.Threading.Tasks;
using DiscomonProject.Discord;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class IntimidateAbility : BasicAbility
    {
        public override string Name { get; } = "Intimidate";
        public override string Description { get; } = "The user is intimidating. When the user enters combat, the enemy's attack is lowered by one stage.";

        public IntimidateAbility()
        {

        }

        public IntimidateAbility(bool newability, BasicMon owner)
        {
            owner.EnteredCombat += mon_EnteredCombat;
        }

        public override async Task mon_EnteredCombat(BasicMon owner, CombatInstance2 inst)
        {
            foreach(BasicMon enemy in inst.GetAllEnemies(owner))
            {
                enemy.ChangeAttStage(-1);
                await MessageHandler.SendMessage(inst.Location, $"{owner.Nickname} intimidates {enemy.Nickname}, lowering their attack by one stage!");
            }
        }

    }
}