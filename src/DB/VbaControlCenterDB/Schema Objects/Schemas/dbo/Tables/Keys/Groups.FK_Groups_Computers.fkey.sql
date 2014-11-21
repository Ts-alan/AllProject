﻿ALTER TABLE [dbo].[Groups]
	ADD CONSTRAINT [FK_Groups_Computers] 
	FOREIGN KEY (ComputerID)
	REFERENCES Computers([ID])
	ON UPDATE CASCADE ON DELETE CASCADE