using System.Runtime.Serialization;

namespace League.Domein.Exceptions
{
    [Serializable]
    internal class SpelerRepositoryADOException : Exception
    {
        public SpelerRepositoryADOException()
        {
        }

        public SpelerRepositoryADOException(string? message) : base(message)
        {
        }

        public SpelerRepositoryADOException(string message, Exception innerException) : base(message, innerException)
        {
        }

        
    }
}