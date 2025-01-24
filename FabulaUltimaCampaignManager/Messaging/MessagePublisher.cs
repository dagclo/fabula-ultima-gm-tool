using System.Collections.Concurrent;
using System.Linq;

namespace FirstProject.Messaging
{
    public class MessagePublisher<T> where T : struct
    {
        private readonly ConcurrentBag<BlockingCollection<IMessage>> _queueCollection;

        public MessagePublisher(ConcurrentBag<BlockingCollection<IMessage>> collection)
        {
            _queueCollection = collection;
        }

        public void Publish(IMessage<T> message)
        {
            foreach(var queue in _queueCollection.Where(q => !q.IsAddingCompleted))
            {
                queue.TryAdd(message);
            }
        }
    }
}
