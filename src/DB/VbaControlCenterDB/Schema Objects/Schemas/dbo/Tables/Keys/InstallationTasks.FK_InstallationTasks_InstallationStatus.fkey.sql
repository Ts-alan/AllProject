ALTER TABLE [dbo].[InstallationTasks]
	ADD CONSTRAINT [FK_InstallationTasks_InstallationStatus] 
	FOREIGN KEY (StatusID)
	REFERENCES InstallationStatus([ID])
	ON UPDATE CASCADE ON DELETE CASCADE