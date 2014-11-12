ALTER TABLE [dbo].[PolicyTypes]
    ADD CONSTRAINT [U_PolicyTypes_TypeName]
    UNIQUE NONCLUSTERED ([TypeName])