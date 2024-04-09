CREATE TABLE BasicAttack
(
	Id TEXT NOT NULL PRIMARY KEY, --GUID
	Name TEXT NOT NULL, 
	Attribute1 TEXT NOT NULL, -- use full name
	Attribute2 TEXT NOT NULL, -- use full name
	IsRanged INTEGER NOT NULL, -- a "boolean"
	DamageType TEXT NOT NULL,
	DamageMod INTEGER NOT NULL,
	AttackMod INTEGER NOT NULL
);

CREATE TABLE BeastAttack
(
	BeastTemplateId TEXT NOT NULL,
	BasicAttackId TEXT NOT NULL,
	PRIMARY KEY (BeastTemplateId, BasicAttackId)
);