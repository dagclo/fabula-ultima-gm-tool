using System;

public struct NpcActionMessage
{
    public Guid Id { get; internal set; }
    public string Action { get; internal set; }
    public string Actor { get; internal set; }
    public string Verb { get; internal set; }
}
