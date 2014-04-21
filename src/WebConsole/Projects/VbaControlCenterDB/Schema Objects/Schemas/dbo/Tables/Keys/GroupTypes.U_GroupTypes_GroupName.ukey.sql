ALTER TABLE [dbo].[GroupTypes]
    ADD CONSTRAINT [U_GroupTypes_GroupName]
    UNIQUE NONCLUSTERED ([GroupName])