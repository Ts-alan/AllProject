﻿ALTER TABLE [dbo].[Events]
	ADD CONSTRAINT [FK_Events_EventTypes] 
	FOREIGN KEY (EventID)
	REFERENCES EventTypes([ID])
	ON UPDATE CASCADE ON DELETE CASCADE