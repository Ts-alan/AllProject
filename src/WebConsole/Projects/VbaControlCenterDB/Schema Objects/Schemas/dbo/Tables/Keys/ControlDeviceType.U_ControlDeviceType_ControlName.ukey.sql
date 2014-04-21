ALTER TABLE [dbo].[ControlDeviceType]
    ADD CONSTRAINT [U_ControlDeviceType_ControlName]
    UNIQUE NONCLUSTERED ([ControlName])