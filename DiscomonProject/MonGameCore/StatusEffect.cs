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
            Confused = false;
            Infatuated = false;
            Flinching = false;
        }
    }
}