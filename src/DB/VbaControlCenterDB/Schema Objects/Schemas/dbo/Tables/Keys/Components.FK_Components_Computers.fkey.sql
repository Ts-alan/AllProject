ALTER TABLE [dbo].[Components]
	ADD CONSTRAINT [FK_Components_Computers] 
	FOREIGN KEY (ComputerID) 
	REFERENCES Computers([ID])
	ON UPDATE CASCADE ON DELETE CASCADE	

