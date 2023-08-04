using System.Runtime.Serialization;

namespace League.Domein.Exceptions
{
    [Serializable]
    internal class TeamRepositoryException : Exception
    {
        public TeamRepositoryException(string message) : base(message)
        {
        }

        public TeamRepositoryException(string? message, Exception innerException) : base(message, innerException)
        {
        }
    }
}