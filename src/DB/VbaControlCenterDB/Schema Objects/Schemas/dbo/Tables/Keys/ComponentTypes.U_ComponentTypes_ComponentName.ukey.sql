ALTER TABLE [dbo].[ComponentTypes]
    ADD CONSTRAINT [U_ComponentTypes_ComponentName]
	UNIQUE NONCLUSTERED ([ComponentName])