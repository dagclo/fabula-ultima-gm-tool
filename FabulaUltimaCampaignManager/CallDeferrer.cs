using System;
using System.Collections.Generic;
using System.Linq;

public class CallDeferrer
{
    private readonly Queue<Action> _callQueue;
    private readonly Action _noOp = () => { };

    public CallDeferrer()
    {
        _callQueue = new Queue<Action>();
    }

    public void Defer(Action action)
    {
        _callQueue.Enqueue(action);
    }

    public Action RunNext()
    {
        if (!_callQueue.Any()) return _noOp;
        return _callQueue.Dequeue();
    }
}