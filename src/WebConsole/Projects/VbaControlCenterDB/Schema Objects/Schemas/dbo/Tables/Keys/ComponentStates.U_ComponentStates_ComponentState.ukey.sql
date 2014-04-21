ALTER TABLE [dbo].[ComponentStates]
    ADD CONSTRAINT [U_ComponentStates_ComponentState]
    UNIQUE NONCLUSTERED ([ComponentState])