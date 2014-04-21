ALTER TABLE [dbo].[InstallationTasks]
	ADD CONSTRAINT [FK_InstallationTasks_InstallationTaskType] 
	FOREIGN KEY (TaskTypeID)
	REFERENCES InstallationTaskType([ID])
	ON UPDATE CASCADE ON DELETE CASCADE