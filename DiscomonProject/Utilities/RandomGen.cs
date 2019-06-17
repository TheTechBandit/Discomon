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
    }
}