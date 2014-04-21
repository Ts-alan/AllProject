ALTER TABLE [dbo].[InstallationTaskType]
    ADD CONSTRAINT [U_InstallationTaskType]
    UNIQUE NONCLUSTERED ([TaskType])