using System.Runtime.Serialization;

namespace League.Domein
{
    [Serializable]
    internal class TransferRepositoryException : Exception
    {
        public TransferRepositoryException(string message):base(message) 
        {
        }

        public TransferRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        
    }
}