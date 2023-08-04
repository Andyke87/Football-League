using System.Runtime.Serialization;

namespace League.Domein
{
    [Serializable]
    internal class TransferManagerException : Exception
    {
        public TransferManagerException(string message): base(message) 
        {
        }

        public TransferManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }

       
    }
}