CREATE TABLE Skills
(
	Id TEXT NOT NULL PRIMARY KEY, --GUID
	Name TEXT NOT NULL, --Species Name
	TargetType TEXT NOT NULL,
	[Text] TEXT NOT NULL,
	IsSpecialRule INTEGER, -- boolean	
	Keywords TEXT NOT NULL, -- JSON Array
	OtherAttributes TEXT NULL -- JSON Object
);

CREATE TABLE BeastSkill
(
	BeastTemplateId TEXT NOT NULL,  --GUID
	SkillId TEXT NOT NULL,  --GUID
	BasicAttackId TEXT NULL  --GUID
);