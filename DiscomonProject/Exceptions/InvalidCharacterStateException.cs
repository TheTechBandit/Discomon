using System;

namespace DiscomonProject
{
    [Serializable]
    class InvalidCharacterStateException: Exception
    {
        public InvalidCharacterStateException()
        {

        }

        public InvalidCharacterStateException(string type)
            : base($"Invalid character state: {type}")
        {

        }
    }
}