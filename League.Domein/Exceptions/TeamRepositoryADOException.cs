using System.Runtime.Serialization;

namespace League.Domein
{
    //[Serializable]
    internal class TeamRepositoryADOException : Exception
    {
        public TeamRepositoryADOException()
        {
        }

        public TeamRepositoryADOException(string? message) : base(message)
        {
        }

        public TeamRepositoryADOException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}