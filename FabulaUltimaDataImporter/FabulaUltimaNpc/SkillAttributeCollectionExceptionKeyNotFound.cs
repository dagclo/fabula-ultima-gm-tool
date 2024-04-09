using System.Runtime.Serialization;

namespace FabulaUltimaNpc
{
    [Serializable]
    internal class SkillAttributeCollectionExceptionKeyNotFound : Exception
    {
        public SkillAttributeCollectionExceptionKeyNotFound()
        {
        }

        public SkillAttributeCollectionExceptionKeyNotFound(string? message) : base(message)
        {
        }

        public SkillAttributeCollectionExceptionKeyNotFound(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SkillAttributeCollectionExceptionKeyNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}