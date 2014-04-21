ALTER TABLE [dbo].[EventTypes]
   ADD CONSTRAINT [DF_EventTypes_NoDelete] 
   DEFAULT 0
   FOR [NoDelete]