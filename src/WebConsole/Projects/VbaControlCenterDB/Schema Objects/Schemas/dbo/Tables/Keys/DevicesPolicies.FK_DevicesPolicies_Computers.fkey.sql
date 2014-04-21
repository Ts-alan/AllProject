ALTER TABLE [dbo].[DevicesPolicies]
	ADD CONSTRAINT [FK_DevicesPolicies_Computers] 
	FOREIGN KEY (ComputerID) 
	REFERENCES Computers([ID])
	ON UPDATE CASCADE ON DELETE CASCADE