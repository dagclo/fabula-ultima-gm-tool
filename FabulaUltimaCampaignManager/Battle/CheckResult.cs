public struct CheckResult
{
    public bool Success { get; set; }
    public int FinalHighRoll { get; set; }
    public int TotalRoll { get; internal set; }
    public int Attribute1Result { get; internal set; }
    public int Attribute2Result { get; internal set; }
    public string Attribute1Name { get; internal set; }
    public string Attribute2Name { get; internal set; }
    public int ResultMod { get; internal set; }
    public int HighRollMod { get; internal set; }
    public int HighRoll { get; internal set; }
    public string Target { get; internal set; }
}
