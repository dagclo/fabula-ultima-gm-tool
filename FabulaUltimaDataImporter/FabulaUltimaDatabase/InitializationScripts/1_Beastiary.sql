CREATE TABLE Species
(
	Id TEXT NOT NULL PRIMARY KEY, --GUID
	Name TEXT NOT NULL, --Species Name
	NumSkills INTEGER NOT NULL,
	NumFreeResistances INTEGER NOT NULL,
	NumFreeImmunities INTEGER NOT NULL,
	NumFreeVulnerabilities INTEGER NOT NULL,
	VulnerabilityChoices TEXT NULL
);

INSERT INTO Species (Id, Name, NumSkills, NumFreeResistances, NumFreeImmunities, NumFreeVulnerabilities, VulnerabilityChoices)
VALUES 
	( 'b0788720-8fa0-4968-ac61-5f3063d97c17', 'Beast', 4, 0, 0, 0, NULL),
	( 'f50815fc-9d41-4eeb-9797-182544244f0a', 'Construct', 2, 0, 0, 0, NULL),
	( '37e76b06-fd97-4c73-8509-eb42e3610eef', 'Demon', 3, 2, 0, 0, NULL),
	( '19014999-30a7-4635-b1a1-505b10a5bc19', 'Elemental', 2, 0, 1, 0, NULL),
	( '69711547-14c6-4a01-af94-f5d5117a6bae', 'Humanoid', 3, 0, 0, 0, NULL),
	( '23e74a9c-8413-497f-b098-f541b43884c0', 'Monster', 4, 0, 0, 0, NULL),
	( 'd608585c-32ff-4d10-88b9-b4df66364195', 'Plant', 3, 0, 0, 1, '["7dfe93bd-67d5-468d-bb32-b4d8c1676305","39fb0c13-06df-47e0-ae4b-ccfb2012b03d","dda401cf-437c-438e-9a1b-e3421f9c4902","01a0f627-748c-49eb-999f-03746b673be5"]'),
	( '3e35bbec-d713-4efc-af8a-3d5e01403885', 'Undead', 1, 0, 0, 1, NULL); --extra skill point granted by free vulnerability


CREATE TABLE BeastTemplate
(
	Id TEXT NOT NULL PRIMARY KEY, --GUID
	Name TEXT NOT NULL,
	Description TEXT NOT NULL,
	Level INTEGER NOT NULL,
	Traits TEXT NULL,
	Species TEXT NOT NULL,
	Dexterity INTEGER NOT NULL, -- listed in dice size
	Insight INTEGER NOT NULL, -- listed in dice size
	Might INTEGER NOT NULL, -- listed in dice size
	Willpower INTEGER NOT NULL, -- listed in dice size	
	ImageFile TEXT NULL
);

CREATE TABLE BeastResistance
(
	BeastTemplateId TEXT NOT NULL,
	DamageTypeId TEXT NOT NULL,
	AffinityId TEXT NOT NULL,
	PRIMARY KEY (BeastTemplateId, DamageTypeId)
);