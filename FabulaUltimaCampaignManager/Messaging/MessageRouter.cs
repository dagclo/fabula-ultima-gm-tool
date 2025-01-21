using Godot;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Messaging
{
    public partial class MessageRouter : Node
    {
        private readonly ConcurrentDictionary<Type, ConcurrentBag<BlockingCollection<IMessage>>> _queueMap 
            = new ConcurrentDictionary<Type, ConcurrentBag<BlockingCollection<IMessage>>>();

        private readonly ConcurrentBag<Task> _runningTasks = new ConcurrentBag<Task>();

        public MessagePublisher<T> GetPublisher<T>() where T : struct
        {
            if(!_queueMap.ContainsKey(typeof(T)))
            {
                _queueMap.TryAdd(typeof(T), new ConcurrentBag<BlockingCollection<IMessage>>());
            }
            return new MessagePublisher<T>(_queueMap[typeof(T)]);
        }

        public Action RegisterSubscriber<TMessageType>(Func<IMessage, Task> func) where TMessageType : struct
        {
            if (!_queueMap.ContainsKey(typeof(TMessageType)))
            {
                _queueMap.TryAdd(typeof(TMessageType), new ConcurrentBag<BlockingCollection<IMessage>>());
            }
            var queue = new BlockingCollection<IMessage>();
            if(_queueMap.TryGetValue(typeof(TMessageType), out var bag))
            {
                bag.Add(queue);
            }
            var task = Task.Run(async () =>
            {
                foreach(var message in queue.GetConsumingEnumerable()) 
                {
                    await func.Invoke(message);
                }
            });
            _runningTasks.Add(task);
            return () =>
            {
                queue.CompleteAdding(); //todo: make it possible to completely remove queues
            };
        }

        public async Task TearDown()
        {
            foreach(var queue in _queueMap.Values.SelectMany(c => c))
            {
                queue.CompleteAdding();
            }

            _queueMap.Clear();

            await Task.WhenAll(_runningTasks.ToArray());            
        }
    }
}
