namespace FabulaUltimaSkillLibrary
{
    [Serializable]
    internal class SkillNotFoundException : Exception
    {
        public SkillNotFoundException()
        {
        }

        public SkillNotFoundException(string? message) : base(message)
        {
        }

        public SkillNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}