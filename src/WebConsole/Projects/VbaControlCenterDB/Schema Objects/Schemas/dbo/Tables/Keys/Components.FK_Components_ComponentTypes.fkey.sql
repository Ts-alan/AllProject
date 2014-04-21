ALTER TABLE [dbo].[Components]
	ADD CONSTRAINT [FK_Components_ComponentTypes] 
	FOREIGN KEY (ComponentID)
	REFERENCES ComponentTypes([ID])
	ON UPDATE CASCADE ON DELETE CASCADE

