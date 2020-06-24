namespace DiscomonProject
{
    public class Environment
    {
        public bool Clear { get; set; } = true;
        public bool Moonrise { get; set; } = false;
        public bool Sunrise { get; set; } = false;
        public bool Heatwave { get; set; } = false;
        public bool Eclipse { get; set; } = false;
        public bool Rain { get; set; } = false;
        public bool Hail { get; set; } = false;
        public bool Sandstorm { get; set; } = false;
        public bool Tailwind { get; set; } = false;
        public bool IonizedAir { get; set; } = false;
        public bool Fog { get; set; } = false;
        public bool Mist { get; set; } = false;
        public bool Sleet { get; set; } = false;
        public bool SevereHailstorm { get; set; } = false;
        public bool Thunderstorm { get; set; } = false;
        public bool Apocalypse { get; set; } = false;
        public int WeatherLevel { get; set; } = 0;

        public Environment()
        {

        }

        public Environment(bool newenv)
        {

        }

        public void ClearAllWeather()
        {
            Clear = true;
            Moonrise = false;
            Sunrise =  false;
            Heatwave = false;
            Eclipse = false;
            Rain = false;
            Hail = false;
            Sandstorm = false;
            Tailwind = false;
            IonizedAir =  false;
            Fog = false;
            Mist = false;
            Sleet = false;
            SevereHailstorm = false;
            Thunderstorm = false;
            Apocalypse = false;
            WeatherLevel = 0;
        }

        public string AttemptMoonrise()
        {
            if(Sunrise)
            {
                Eclipse = true;
                Sunrise = false;
                Clear = false;
                WeatherLevel = 2;
                return "The moon eclipses the sun!";
            }
            else if(WeatherLevel >= 2)
            {
                return $"The {WeatherNameToString()} prevented moonrise!";
            }

            Moonrise = true;
            Clear = false;
            WeatherLevel = 1;
            return "The moon rises!";
        }

        public string WeatherToString()
        {
            if(Moonrise)
                return "The moonlight shines!";
            if(Sunrise)
                return "The sun shines!";
            if(Heatwave)
                return "The heat is overwhelming!";
            if(Eclipse)
                return "The battlefield is dark.";
            if(Rain)
                return "It is raining!";
            if(Hail)
                return "It is hailing!";
            if(Sandstorm)
                return "A sandstorm rages!";
            if(Tailwind)
                return "The wind blows!";
            if(IonizedAir)
                return "The air is crackling with static!";
            if(Fog)
                return "The battlefield is foggy...";
            if(Mist)
                return "The battlefield is misty...";
            if(Sleet)
                return "Ice-cold water pours from the sky!";
            if(SevereHailstorm)
                return "It is severely hailing!";
            if(Thunderstorm)
                return "The storm rages!";
            if(Apocalypse)
                return "The apocalypse is here.";
            return "";
        }

        public string WeatherNameToString()
        {
            if(Clear)
                return "clear skies";
            if(Moonrise)
                return "moonrise";
            if(Sunrise)
                return "sunrise";
            if(Heatwave)
                return "heatwave";
            if(Eclipse)
                return "eclipse";
            if(Rain)
                return "rain";
            if(Hail)
                return "hail";
            if(Sandstorm)
                return "sandstorm";
            if(Tailwind)
                return "tailwind";
            if(IonizedAir)
                return "ionized air";
            if(Fog)
                return "fog";
            if(Mist)
                return "mist";
            if(Sleet)
                return "sleet";
            if(SevereHailstorm)
                return "severe hailstorm";
            if(Thunderstorm)
                return "thunderstorm";
            if(Apocalypse)
                return "apocalypse";
            return "";
        }
    }
}