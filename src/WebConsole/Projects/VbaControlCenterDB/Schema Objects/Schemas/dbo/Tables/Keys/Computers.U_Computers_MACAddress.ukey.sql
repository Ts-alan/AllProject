ALTER TABLE [dbo].[Computers]
    ADD CONSTRAINT [U_Computers_MACAddress]
    UNIQUE NONCLUSTERED ([MACAddress])