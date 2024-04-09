using FirstProject.Encounters;
using System.Collections.Generic;

public interface IEncounterReader
{
	void ReadEncounter(Encounter encounter, IReadOnlyList<BattleStatus> battleStatuses);
}