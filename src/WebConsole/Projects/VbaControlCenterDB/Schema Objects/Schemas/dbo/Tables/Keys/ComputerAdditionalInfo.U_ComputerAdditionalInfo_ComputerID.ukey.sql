ALTER TABLE [dbo].[ComputerAdditionalInfo]
    ADD CONSTRAINT [U_ComputerAdditionalInfo_ComputerID]
	UNIQUE NONCLUSTERED ([ComputerID])