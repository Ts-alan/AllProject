﻿ALTER TABLE [dbo].[Tasks]
	ADD CONSTRAINT [FK_Tasks_Computers] 
	FOREIGN KEY (ComputerID)
	REFERENCES Computers([ID])
	ON UPDATE CASCADE ON DELETE CASCADE