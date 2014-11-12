ALTER TABLE [dbo].[ScanningComments]
    ADD CONSTRAINT [U_ScanningComments_IP]
    UNIQUE NONCLUSTERED ([IPAddress])