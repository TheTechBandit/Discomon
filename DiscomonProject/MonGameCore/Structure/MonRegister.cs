using System;

namespace DiscomonProject
{
    public static class MonRegister
    {
        static MonRegister()
        {

        }

        public static BasicMon StringToMonRegister(string str)
        {
            BasicMon mon;
            str = str.ToLower();

            switch(str)
            {
                case "suki":
                    mon = new Suki(true);
                    break;
                case "snoril":
                    mon = new Snoril(true);
                    break;
                case "smoledge":
                    mon = new Smoledge(true);
                    break;
                case "grasipup":
                    mon = new Grasipup(true);
                    break;
                case "arness":
                    mon = new Arness(true);
                    break;
                case "meliosa":
                    mon = new Meliosa(true);
                    break;
                case "ritala":
                    mon = new Ritala(true);
                    break;
                case "elesoak":
                    mon = new Elesoak(true);
                    break;
                case "elecute":
                    mon = new Elecute(true);
                    break;
                case "psygoat":
                    mon = new Psygoat(true);
                    break;
                case "ook":
                    mon = new Ook(true);
                    break;
                case "stebble":
                    mon = new Stebble(true);
                    break;
                case "sedimo":
                    mon = new Sedimo(true);
                    break;
                default:
                    mon = new Snoril(true);
                    break;
            }
            
            return mon;
        }
        
    }
}