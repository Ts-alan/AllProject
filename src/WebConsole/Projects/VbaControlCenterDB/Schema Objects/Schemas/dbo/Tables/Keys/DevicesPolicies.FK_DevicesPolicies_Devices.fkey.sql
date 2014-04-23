﻿ALTER TABLE [dbo].[DevicesPolicies]
	ADD CONSTRAINT [FK_DevicesPolicies_Devices] 
	FOREIGN KEY (DeviceID) 
	REFERENCES Devices([ID])
	ON UPDATE CASCADE ON DELETE CASCADE