using System;
using System.Threading.Tasks;
using DiscomonProject.Discord;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class NoneAbility : BasicAbility
    {
        public override string Name { get; } = "None";
        public override string Description { get; } = "No Ability";

        public NoneAbility()
        {

        }

        public NoneAbility(bool newability, BasicMon owner)
        {

        }

    }
}