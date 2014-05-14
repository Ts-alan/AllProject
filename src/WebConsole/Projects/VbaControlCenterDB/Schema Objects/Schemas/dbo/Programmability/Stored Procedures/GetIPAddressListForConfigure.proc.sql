CREATE PROCEDURE [dbo].[GetIPAddressListForConfigure]
WITH ENCRYPTION
AS
	SELECT DISTINCT(t.[IPAddress]), s.[Status] FROM InstallationTasks AS t	
	INNER JOIN InstallationStatus AS s ON s.[ID] = t.[StatusID]
	WHERE [IPAddress] NOT IN (SELECT [IPAddress] FROM Computers) AND s.[Status] = 'Success'