using System.Runtime.Serialization;

namespace League.Domein.Exceptions
{
    //[Serializable]
    public class SpelerException : Exception
    {
        public SpelerException(string message) : base(message)
        {
        }

        public SpelerException(string message, Exception innerException) : base(message, innerException)
        {
        }


    }
}