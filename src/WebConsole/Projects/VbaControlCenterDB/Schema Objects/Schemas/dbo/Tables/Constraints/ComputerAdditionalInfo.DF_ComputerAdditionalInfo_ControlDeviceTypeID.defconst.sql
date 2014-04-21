ALTER TABLE [dbo].[ComputerAdditionalInfo]
   ADD CONSTRAINT [DF_ComputerAdditionalInfo_ControlDeviceTypeID] 
   DEFAULT 1
   FOR ControlDeviceTypeID
