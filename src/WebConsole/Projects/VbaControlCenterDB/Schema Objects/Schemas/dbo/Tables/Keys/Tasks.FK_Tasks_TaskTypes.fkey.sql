﻿ALTER TABLE [dbo].[Tasks]
	ADD CONSTRAINT [FK_Tasks_TaskTypes] 
	FOREIGN KEY (TaskID)
	REFERENCES TaskTypes([ID])
	ON UPDATE CASCADE ON DELETE CASCADE