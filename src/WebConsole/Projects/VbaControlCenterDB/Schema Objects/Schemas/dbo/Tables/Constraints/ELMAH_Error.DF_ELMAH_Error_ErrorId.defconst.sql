﻿ALTER TABLE [dbo].[ELMAH_Error]
   ADD CONSTRAINT [DF_ELMAH_Error_ErrorId] 
   DEFAULT (NEWID()) 
   FOR [ErrorId]