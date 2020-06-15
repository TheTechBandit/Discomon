using System.Threading.Tasks;

namespace DiscomonProject
{
    public class StatusEffect : BasicMove
    {
        public bool Paraylzed { get; set; } = false;
        public bool Burned { get; set; } = false;
        public bool Poisoned { get; set; } = false;
        public bool BadlyPoisoned { get; set; } = false;
        public bool Frozen { get; set; } = false;
        public bool Asleep { get; set; } = false;
        public int SleepTurns { get; set; } = 0;
        public bool Confused { get; set; } = false;
        public bool Infatuated { get; set; } = false;
        public bool Flinching { get; set; } = false;

        public StatusEffect() :base()
        {

        }

        public StatusEffect(bool newstatus) 
        {

        }

        public void CureAll()
        {
            Paraylzed = false;
            Burned = false;
            Poisoned = false;
            BadlyPoisoned = false;
            Frozen = false;
            Asleep = false;
            SleepTurns = 0;
            Confused = false;
            Infatuated = false;
            Flinching = false;
        }

        public void FallAsleep(int number)
        {
            Asleep = true;
            if(number < 1)
            {
                SleepTurns = RandomGen.RandomInt(1, 3);
            }
            else
            {
                SleepTurns = number;
            }
        }
 
        public void SleepTick()
        {
            SleepTurns--;
            if(SleepTurns <= 0)
                Asleep = false;
        }

        //Returns true if unfrozen
        public bool FreezeTick()
        {
            if(RandomGen.PercentChance(20.0))
            {
                Frozen = false;
                return true;
            }
            return false;
        }
    }
}