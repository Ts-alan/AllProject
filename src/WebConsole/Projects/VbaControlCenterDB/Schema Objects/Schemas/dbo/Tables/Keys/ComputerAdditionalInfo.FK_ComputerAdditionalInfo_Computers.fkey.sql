ALTER TABLE [dbo].[ComputerAdditionalInfo]
	ADD CONSTRAINT [FK_ComputerAdditionalInfo_Computers] 
	FOREIGN KEY ([ComputerID])
	REFERENCES Computers([ID])
	ON UPDATE CASCADE ON DELETE CASCADE
