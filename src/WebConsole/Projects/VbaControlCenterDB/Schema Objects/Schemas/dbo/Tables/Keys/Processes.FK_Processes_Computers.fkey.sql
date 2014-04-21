﻿ALTER TABLE [dbo].[Processes]
	ADD CONSTRAINT [FK_Processes_Computers] 
	FOREIGN KEY (ComputerID)
	REFERENCES Computers([ID])
	ON UPDATE CASCADE ON DELETE CASCADE