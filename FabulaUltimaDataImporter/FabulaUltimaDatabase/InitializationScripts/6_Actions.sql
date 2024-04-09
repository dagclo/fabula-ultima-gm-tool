CREATE TABLE [Action]
(
	Id TEXT NOT NULL PRIMARY KEY, --GUID
	Name TEXT NOT NULL, 
	Effect TEXT NOT NULL
);

CREATE TABLE BeastAction
(
	BeastTemplateId TEXT NOT NULL,
	ActionId TEXT NOT NULL,
	PRIMARY KEY (BeastTemplateId, ActionId)
);