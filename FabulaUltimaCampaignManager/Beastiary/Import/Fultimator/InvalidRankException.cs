using System;
using System.Runtime.Serialization;

namespace FabulaUltimaGMTool.Beastiary.Import.Fultimator
{
    [Serializable]
    internal class InvalidRankException : Exception
    {
        public InvalidRankException()
        {
        }

        public InvalidRankException(string message) : base(message)
        {
        }

        public InvalidRankException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidRankException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}