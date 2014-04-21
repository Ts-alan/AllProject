ALTER TABLE [dbo].[GroupTypes]
	ADD CONSTRAINT [FK_GroupTypes_GroupTypes] 
	FOREIGN KEY (ParentID)
	REFERENCES GroupTypes([ID])