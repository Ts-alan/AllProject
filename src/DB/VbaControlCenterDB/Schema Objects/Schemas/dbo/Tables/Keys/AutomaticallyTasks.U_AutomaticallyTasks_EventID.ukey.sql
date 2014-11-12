ALTER TABLE [dbo].[AutomaticallyTasks]
    ADD CONSTRAINT [U_AutomaticallyTasks_EventID]
    UNIQUE NONCLUSTERED (EventID)