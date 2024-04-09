// See https://aka.ms/new-console-template for more information

using System.Runtime.Serialization;

[Serializable]
internal class SkillWorkFlowException : Exception
{
    public SkillWorkFlowException()
    {
    }

    public SkillWorkFlowException(string? message) : base(message)
    {
    }

    public SkillWorkFlowException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected SkillWorkFlowException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}