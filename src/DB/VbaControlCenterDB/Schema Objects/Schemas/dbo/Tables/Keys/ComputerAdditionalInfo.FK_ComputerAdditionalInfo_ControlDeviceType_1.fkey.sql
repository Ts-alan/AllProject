ALTER TABLE [dbo].[ComputerAdditionalInfo]
	ADD CONSTRAINT [FK_ComputerAdditionalInfo_ControlDeviceType] 
	FOREIGN KEY ([ControlDeviceTypeID]) 
	REFERENCES ControlDeviceType([ID])
	ON UPDATE CASCADE ON DELETE CASCADE
