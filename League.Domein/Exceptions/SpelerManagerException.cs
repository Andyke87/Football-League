using System.Runtime.Serialization;

namespace League.Domein
{
   // [Serializable]
    internal class SpelerManagerException : Exception
    {
        public SpelerManagerException(string message):base(message) 
        {
        }

        public SpelerManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }

      
    }
}