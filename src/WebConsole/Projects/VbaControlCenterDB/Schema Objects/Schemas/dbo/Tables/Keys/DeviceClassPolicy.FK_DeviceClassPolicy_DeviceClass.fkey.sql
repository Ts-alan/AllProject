ALTER TABLE [dbo].[DeviceClassPolicy]
	ADD CONSTRAINT [FK_DeviceClassPolicy_DeviceClass] 
	FOREIGN KEY (DeviceClassID) 
	REFERENCES DeviceClass([ID])
	ON UPDATE CASCADE ON DELETE CASCADE