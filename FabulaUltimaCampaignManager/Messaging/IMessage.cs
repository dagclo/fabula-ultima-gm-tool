using System;

namespace FirstProject.Messaging
{
    public interface IMessage
    {
        Guid Id { get; }
    }

    public interface IMessage<T> : IMessage where T : struct
    {
        T Value { get; }
    }
}
