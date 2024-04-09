CREATE TABLE Dice
(
	Sides INTEGER NOT NULL PRIMARY KEY,
	Name TEXT NOT NULL	
);

INSERT INTO Dice (Sides, Name)
VALUES 
	( 6, 'D6'),
	( 8, 'D8'),
	( 10, 'D10'),
	( 12, 'D12'),
	( 20, 'D20');

CREATE TABLE Affinity
(
	Id TEXT NOT NULL PRIMARY KEY, --GUID
	Name TEXT NOT NULL, --Name for text
	ShortName TEXT NOT NULL --Name for text
);


INSERT INTO Affinity (Id, Name, ShortName)
VALUES 
	( '77a9c176-4ddb-46ef-a945-0023bf7a6f6b', 'No Affinity', ''),
	( 'fc15b76a-6505-46f4-81ad-da4a5bf1bd42', 'Vulnerable', 'VU'),
	( '75059ae9-e6c7-4d45-a1f1-18bf617b0d2a', 'Resistant', 'RS'),
	( '1d843905-9d75-4c57-84c2-07dc7d179d3b', 'Immune', 'IM'),
	( '40058e84-b555-4943-bf4d-5c7381272749', 'Absorbs', 'AB');


CREATE TABLE DamageTypes
(
	Id TEXT NOT NULL PRIMARY KEY, --GUID
	Name TEXT NOT NULL, --Name for text
	SkillBonus INTEGER NOT NULL -- skill points gained for vulenerability
);


INSERT INTO DamageTypes (Id, Name, SkillBonus)
VALUES 
	( 'ffad483e-0ad5-4a43-b235-080ddfd67470', 'Physical', 2),
	( '7dfe93bd-67d5-468d-bb32-b4d8c1676305', 'Air', 1),
	( '39fb0c13-06df-47e0-ae4b-ccfb2012b03d', 'Bolt', 1),
	( 'c635cee1-fcbc-44cd-98c9-fe55c7084806', 'Dark', 1),
	( '813f85c6-fa28-42f2-ad4b-b682a4814382', 'Earth', 1),
	( 'dda401cf-437c-438e-9a1b-e3421f9c4902', 'Fire', 1),
	( '01a0f627-748c-49eb-999f-03746b673be5', 'Ice', 1),
	( '9ef9cb1e-96da-4acc-ae0e-66e8e5236888', 'Light', 1),
	( 'f36c11bf-a896-4cc7-9460-37bf5100e14a', 'Poison', 1),
	( '3da973fa-c8fa-4eaa-9fdc-8cda9fc0c5af', 'No Damage', 0);
	