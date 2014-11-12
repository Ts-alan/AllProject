ALTER TABLE [dbo].[EventTypes]
   ADD CONSTRAINT [DF_EventTypes_Send] 
   DEFAULT 0
   FOR [Send]