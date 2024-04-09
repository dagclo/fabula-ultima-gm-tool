using System.Runtime.Serialization;

namespace FabulaUltimaDatabase.Configuration
{
    [Serializable]
    internal class NoDatabaseFileException : Exception
    {
        public string? MissingFile { get; }

        public NoDatabaseFileException(string? filename) : base($"{filename} not found")
        {
            MissingFile = filename;
        }

        public NoDatabaseFileException(string? filename, Exception? innerException) : base($"{filename} not found", innerException)
        {
            MissingFile = filename;
        }

        protected NoDatabaseFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}