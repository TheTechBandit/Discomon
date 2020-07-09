using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class BasicAbility
    {
        public virtual string Name { get; }
        public virtual string Description { get; }

        public BasicAbility()
        {

        }

        public BasicAbility(bool newability, BasicMon owner)
        {

        }

        public virtual async Task mon_EnteredCombat(BasicMon owner, CombatInstance2 inst)
        {
            await Task.Run(null);
        }

    }
}