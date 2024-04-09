using System;

namespace FirstProject.Messaging
{
    public record Message<T> : IMessage<T> where T : struct
    {
        public required T Value { get; init; }

        public required Guid Id { get; init; }
    }

    public static class MessageExtensions
    {
        public static IMessage<T> AsMessage<T>(this T obj) where T : struct 
        {
            return new Message<T>
            {
                Value = obj,
                Id = Guid.NewGuid()
            };
        }
    }
}
