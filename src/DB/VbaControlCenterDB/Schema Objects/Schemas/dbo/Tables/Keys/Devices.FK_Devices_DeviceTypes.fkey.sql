﻿ALTER TABLE [dbo].[Devices]
	ADD CONSTRAINT [FK_Devices_DeviceTypes] 
	FOREIGN KEY (DeviceTypeID) 
	REFERENCES DeviceTypes([ID])
	ON UPDATE CASCADE ON DELETE CASCADE