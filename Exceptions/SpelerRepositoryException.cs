using System.Runtime.Serialization;

namespace League.Domein.Exceptions
{
    [Serializable]
    internal class SpelerRepositoryException : Exception
    {
        public SpelerRepositoryException(string message) : base(message)
        {
        }

        public SpelerRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }


    }
}