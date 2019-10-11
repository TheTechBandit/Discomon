using System;

namespace DiscomonProject.Utilities
{
    public static class RandomGen
    {
        public static Random Gen { get; }

        static RandomGen() 
            => Gen = new Random();
    }
}