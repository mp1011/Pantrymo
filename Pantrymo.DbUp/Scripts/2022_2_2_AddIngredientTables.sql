CREATE TABLE Components
(
	Id INTEGER PRIMARY KEY, 
	[Name] TEXT NOT NULL,	
	NonComponent INTEGER NOT NULL,
	Assumed INTEGER  NOT NULL,
	MasterCategory INTEGER  NOT NULL,
	SubCategory INTEGER NOT NULL,
	LastModified TEXT NOT NULL
);

CREATE TABLE AlternateComponentNames
(
	Id INTEGER PRIMARY KEY,
	AlternateName TEXT NOT NULL,
	ComponentId INTEGER NOT NULL,
	LastModified TEXT NOT NULL
);

