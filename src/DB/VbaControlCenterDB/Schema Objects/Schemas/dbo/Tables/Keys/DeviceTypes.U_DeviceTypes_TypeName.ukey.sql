ALTER TABLE [dbo].[DeviceTypes]
    ADD CONSTRAINT [U_DeviceTypes_TypeName]
    UNIQUE NONCLUSTERED ([TypeName])