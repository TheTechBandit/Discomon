using System;
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
        public int Sleepy { get; set; } = 0;
        public int SleepTurns { get; set; } = 0;
        public bool Confused { get; set; } = false;
        public bool Infatuated { get; set; } = false;
        public bool Flinching { get; set; } = false;
        public bool Flying { get; set; } = false;
        public bool Charged { get; set; } =  false;

        public StatusEffect() :base()
        {

        }

        public StatusEffect(bool newstatus) 
        {

        }

        public void StatusCure()
        {
            Paraylzed = false;
            Burned = false;
            Poisoned = false;
            BadlyPoisoned = false;
            Frozen = false;
            Asleep = false;
            Sleepy = 0;
            SleepTurns = 0;
        }

        public void CureAll()
        {
            StatusCure();
            Confused = false;
            Infatuated = false;
            Flinching = false;
        }

        public void ResetAll()
        {
            CureAll();
            Flying = false;
            Charged = false;
        }

        public void CombatReset()
        {
            Confused = false;
            Infatuated = false;
            Flinching = false;
            Flying = false;
            Charged = false;
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

        //Returns true if awake
        public bool SleepTick()
        {
            SleepTurns--;
            if(SleepTurns <= 0)
            {
                Asleep = false;
                return true;
            }
            return false;
        }

        //Returns true if unfrozen
        public bool FreezeTick()
        {
            if(RandomGen.PercentChance(20.0))
            {
                Console.WriteLine("freeze tick");
                Frozen = false;
                return true;
            }
            return false;
        }
    }
}