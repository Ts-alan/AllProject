﻿ALTER TABLE [dbo].[Events]
	ADD CONSTRAINT [FK_Events_Computers] 
	FOREIGN KEY (ComputerID)
	REFERENCES Computers([ID])
	ON UPDATE CASCADE ON DELETE CASCADE