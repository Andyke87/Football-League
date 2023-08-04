using System.Runtime.Serialization;

namespace League.Domein.Exceptions
{
    //[Serializable]
    public class TransferException : Exception
    {
        public TransferException(string message) : base(message)
        {

        }

        public TransferException(string message, Exception innerException) : base(message, innerException)
        {

        }


    }
}