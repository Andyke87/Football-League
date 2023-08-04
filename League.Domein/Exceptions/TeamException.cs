using System.Runtime.Serialization;

namespace League.Domein
{
   // [Serializable]
    public class TeamException : Exception
    {
        public TeamException(string message) : base(message) 
        {

        }

        public TeamException(string message, Exception innerException) : base(message, innerException)
        {

        }

        
    }
}