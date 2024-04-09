CREATE TABLE Equipment
(
	Id TEXT NOT NULL PRIMARY KEY, --GUID
	Name TEXT NOT NULL,
	CategoryId TEXT NOT NULL,
	Attribute1 TEXT NULL, -- use full name
	Attribute2 TEXT NULL, -- use full name
	Cost INTEGER NOT NULL, 
	DamageType TEXT NULL,
	DamageMod INTEGER NULL,
	AttackMod INTEGER NULL,	
	NumHands INTEGER NULL,	
	IsMartial INTEGER NOT NULL, -- a "boolean"
	Quality TEXT NULL,
	DefenseModification INTEGER NULL,
	DefenseOverride INTEGER NULL,
	MagicDefenceModification INTEGER NULL,
	InitiativeModification INTEGER NULL
);

CREATE TABLE BeastEquipment
(
	BeastTemplateId TEXT NOT NULL,
	EquipmentId TEXT NOT NULL,
	PRIMARY KEY (BeastTemplateId, EquipmentId)
);

CREATE TABLE EquipmentCategory
(
	Id TEXT NOT NULL PRIMARY KEY, --GUID
	Name TEXT NOT NULL, --Name for text
	IsWeapon INTEGER NOT NULL, -- a "boolean"
	IsArmor INTEGER NOT NULL, -- a "boolean"
	IsRanged INTEGER NOT NULL -- a "boolean"
);


INSERT INTO EquipmentCategory (Id, Name, IsWeapon, IsArmor, IsRanged)
VALUES 
	( '95c008d0-0360-4a58-97f9-ad61cdaad60c', 'Arcane', 1, 0, 0),
	( '0e27bf2b-25e6-4080-8cf7-1c59c50b4d2e', 'Armor', 0, 1, 0),
	( 'a1d69dd6-b0ca-4fe9-9359-d59949898306', 'Bow', 1, 0, 1),
	( '9599f6a5-f432-4989-936d-532b28303265', 'Brawling', 1, 0, 0),
	( '29b695eb-5d11-40e1-9288-d0279a752786', 'Dagger', 1, 0, 0),
	( 'bcd03648-743e-4ba6-836c-6baee226ad7b', 'Firearm', 1, 0, 1),
	( '75b05c8b-f7e2-4dd0-ada2-bfe172d5169c', 'Flail', 1, 0, 0),
	( 'f4d05e86-65a5-4f74-ba2b-da137a5ad952', 'Heavy', 1, 0, 0),
	( '0ecfea40-2859-45f6-bf78-0edd4f8b7d06', 'Spear', 1, 0, 0),
	( '74313314-cc72-4cfb-8b43-3a673c9cdd72', 'Sword', 1, 0, 0),
	( '99e72dcc-0bbc-4a3b-978b-8fa478a21d53', 'Thrown', 1, 0, 1),
	( '648c285d-9848-4152-b238-882859ebdbb8', 'Shield', 0, 1, 0),
	( '7878311d-bad6-4de8-9318-79419b361631', 'Accessory', 0, 0, 0);