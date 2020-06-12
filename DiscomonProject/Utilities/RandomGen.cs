using System;

namespace DiscomonProject
{
    public static class RandomGen
    {
        public static Random Gen { get; }

        static RandomGen()
        {
            Gen = new Random();
        }

        public static double RandomDouble(double min, double max)
        {
            var random = Gen.NextDouble() * (max - min) + min;
            Console.WriteLine($"Random double between {min} and {max}: {random}");
            return random;
        }

        public static bool PercentChance(double chance)
        {
            if(RandomDouble(1.0, 100.0) <= chance)
            {
                return true;
            }
            else return false;
        }
    }
}