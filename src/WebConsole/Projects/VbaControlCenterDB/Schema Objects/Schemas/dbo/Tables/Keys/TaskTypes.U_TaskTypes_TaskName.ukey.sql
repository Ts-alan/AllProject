ALTER TABLE [dbo].[TaskTypes]
    ADD CONSTRAINT [U_TaskTypes_TaskName]
    UNIQUE NONCLUSTERED ([TaskName])