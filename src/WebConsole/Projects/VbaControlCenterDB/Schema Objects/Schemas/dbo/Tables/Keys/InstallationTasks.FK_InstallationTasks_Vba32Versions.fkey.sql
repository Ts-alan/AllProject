ALTER TABLE [dbo].[InstallationTasks]
	ADD CONSTRAINT [FK_InstallationTasks_Vba32Versions] 
	FOREIGN KEY (Vba32VersionID)
	REFERENCES Vba32Versions([ID])
	ON UPDATE CASCADE ON DELETE CASCADE