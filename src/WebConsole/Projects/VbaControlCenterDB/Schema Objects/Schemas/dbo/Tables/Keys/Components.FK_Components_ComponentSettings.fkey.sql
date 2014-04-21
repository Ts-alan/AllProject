ALTER TABLE [dbo].[Components]
	ADD CONSTRAINT [FK_Components_ComponentSettings] 
	FOREIGN KEY (SettingsID)
	REFERENCES ComponentSettings([ID])
	ON UPDATE CASCADE ON DELETE CASCADE