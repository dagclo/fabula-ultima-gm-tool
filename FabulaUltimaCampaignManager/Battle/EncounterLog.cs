using System;

public struct EncounterLog
{
    public Guid Id { get; internal set; }
    public string Action { get; internal set; }
    public string Actor { get; internal set; }
    public string Verb { get; internal set; }
    public DisplayLevel DisplayLevel { get; internal set; }

    public override string ToString()
    {
        return $"{Actor} {Verb} {Action}";
    }
}

public enum DisplayLevel
{
    DEFAULT,
    WHOOSH,
    CELEBRATE
}

public struct EncounterEnd
{

}
