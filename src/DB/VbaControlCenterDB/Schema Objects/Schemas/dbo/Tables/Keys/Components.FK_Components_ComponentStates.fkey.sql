ALTER TABLE [dbo].[Components]
	ADD CONSTRAINT [FK_Components_ComponentStates] 
	FOREIGN KEY (StateID)
	REFERENCES ComponentStates([ID])
	ON UPDATE CASCADE ON DELETE CASCADE

