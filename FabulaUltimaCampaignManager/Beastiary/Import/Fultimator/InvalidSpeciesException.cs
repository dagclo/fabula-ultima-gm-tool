using System;
using System.Runtime.Serialization;

namespace FabulaUltimaGMTool.Beastiary.Import.Fultimator
{
    [Serializable]
    internal class InvalidSpeciesException : Exception
    {
        public InvalidSpeciesException()
        {
        }

        public InvalidSpeciesException(string message) : base(message)
        {
        }

        public InvalidSpeciesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidSpeciesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}