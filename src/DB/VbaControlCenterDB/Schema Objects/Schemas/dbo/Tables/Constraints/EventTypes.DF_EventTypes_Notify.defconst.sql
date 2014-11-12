ALTER TABLE [dbo].[EventTypes]
   ADD CONSTRAINT [DF_EventTypes_Notify] 
   DEFAULT 0
   FOR [Notify]