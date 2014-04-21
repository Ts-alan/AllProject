ALTER TABLE [dbo].[Components]
	ADD CONSTRAINT [PK_Components]
	PRIMARY KEY NONCLUSTERED ([ComputerID], [ComponentID])