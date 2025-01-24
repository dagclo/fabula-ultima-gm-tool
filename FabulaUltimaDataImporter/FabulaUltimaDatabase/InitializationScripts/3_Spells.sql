CREATE TABLE Spell
(
	Id TEXT NOT NULL PRIMARY KEY, --GUID
	Name TEXT NOT NULL, 
	Duration TEXT NOT NULL,
	Target TEXT NOT NULL,
	MagicPointCost INTEGER NOT NULL, 
	Description TEXT NOT NULL,
	Attribute1 TEXT NULL, -- use full name
	Attribute2 TEXT  NULL, -- use full name
	IsOffensive INTEGER NOT NULL, -- a "boolean"
	DamageType TEXT NULL, -- GUID reference to damage types
	DamageModifier INTEGER NULL
);

CREATE TABLE BeastSpell
(
	BeastTemplateId TEXT NOT NULL,
	SpellId TEXT NOT NULL,
	PRIMARY KEY (BeastTemplateId, SpellId)
);