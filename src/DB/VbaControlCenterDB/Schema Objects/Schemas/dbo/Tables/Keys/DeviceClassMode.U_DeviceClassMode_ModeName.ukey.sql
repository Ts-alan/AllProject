ALTER TABLE [dbo].[DeviceClassMode]
    ADD CONSTRAINT [U_DeviceClassMode_ModeName]
    UNIQUE NONCLUSTERED ([ModeName])