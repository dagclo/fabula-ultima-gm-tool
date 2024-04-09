public struct CheckResult
{
    public bool Success { get; set; }
    public int FinalHighRoll { get; set; }
    public int TotalRoll { get; internal set; }
    public int Attribute1Result { get; internal set; }
    public int Attribute2Result { get; internal set; }
}
