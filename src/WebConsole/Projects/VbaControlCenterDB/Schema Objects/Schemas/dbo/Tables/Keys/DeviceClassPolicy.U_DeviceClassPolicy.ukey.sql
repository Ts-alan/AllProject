ALTER TABLE [dbo].[DeviceClassPolicy]
    ADD CONSTRAINT [U_DeviceClassPolicy]
    UNIQUE NONCLUSTERED ([ComputerID], [DeviceClassID])