ALTER TABLE [dbo].[DeviceClassPolicy]
	ADD CONSTRAINT [FK_DeviceClassPolicy_Computers] 
	FOREIGN KEY (ComputerID)
	REFERENCES Computers([ID])
	ON UPDATE CASCADE ON DELETE CASCADE