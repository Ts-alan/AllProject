ALTER TABLE [dbo].[DevicePolicyStates]
    ADD CONSTRAINT [U_DevicePolicyStates_StateName]
    UNIQUE NONCLUSTERED ([StateName])