using System.Runtime.Serialization;

namespace League.Domein
{
    [Serializable]
    internal class TeamManagerException : Exception
    {
        public TeamManagerException(string message):base(message) 
        {
        }

        public TeamManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
       
    }
}