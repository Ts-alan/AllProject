ALTER TABLE [dbo].[DeviceClassPolicy]
	ADD CONSTRAINT [FK_DeviceClassPolicy_DeviceClassMode] 
	FOREIGN KEY (DeviceClassModeID)
	REFERENCES DeviceClassMode([ID])
	ON UPDATE CASCADE ON DELETE CASCADE