namespace FabulaUltimaSkillLibrary
{
    [Serializable]
    internal class InvalidDefenseBoostException : Exception
    {
        private int mDefMod;
        private int defMod;
        private int? defOverride;

        public InvalidDefenseBoostException()
        {
        }

        public InvalidDefenseBoostException(string? message) : base(message)
        {
        }

        public InvalidDefenseBoostException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public InvalidDefenseBoostException(int mDefMod, int defMod, int? defOverride)
        {
            this.mDefMod = mDefMod;
            this.defMod = defMod;
            this.defOverride = defOverride;
        }
    }
}