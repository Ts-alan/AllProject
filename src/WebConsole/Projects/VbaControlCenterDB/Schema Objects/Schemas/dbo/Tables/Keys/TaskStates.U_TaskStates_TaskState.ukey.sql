ALTER TABLE [dbo].[TaskStates]
    ADD CONSTRAINT [U_TaskStates_TaskState]
    UNIQUE NONCLUSTERED ([TaskState])