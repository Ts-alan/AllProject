ALTER TABLE [dbo].[EventTypes]
    ADD CONSTRAINT [U_EventTypes_EventName]
    UNIQUE NONCLUSTERED ([EventName])