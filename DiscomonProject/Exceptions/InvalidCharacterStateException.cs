using System;

namespace DiscomonProject.Exceptions
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