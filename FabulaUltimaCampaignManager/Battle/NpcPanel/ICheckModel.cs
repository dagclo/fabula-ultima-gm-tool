using FabulaUltimaNpc;
using System;

public interface ICheckModel
{
	string Action { get; set; }
	Die Attribute1Die { get; set; }
    string Attribute1Name { get; set; }
    Die Attribute2Die { get; set; }
    string Attribute2Name { get; set; }
    int? AccuracyMod { get; set; }
	int Difficulty { get; set; }
	int? HighRollMod { get; set; }
    Action Changed { get; set; }
    bool IsValid { get; }
}
