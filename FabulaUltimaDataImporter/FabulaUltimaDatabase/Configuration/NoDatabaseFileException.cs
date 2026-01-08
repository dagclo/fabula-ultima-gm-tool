namespace FabulaUltimaDatabase.Configuration
{
    [Serializable]
    public class NoDatabaseFileException : Exception
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
    }
}