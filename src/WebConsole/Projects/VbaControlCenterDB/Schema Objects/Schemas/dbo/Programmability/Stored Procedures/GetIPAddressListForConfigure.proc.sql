CREATE PROCEDURE [dbo].[GetIPAddressListForConfigure]
WITH ENCRYPTION
AS
	SELECT DISTINCT(t.[IPAddress]), s.[Status], tt.[TaskType] FROM InstallationTasks AS t
	INNER JOIN InstallationTaskType AS tt ON tt.[ID] = t.[TaskTypeID]
	INNER JOIN InstallationStatus AS s ON s.[ID] = t.[StatusID]
	WHERE [IPAddress] NOT IN (SELECT [IPAddress] FROM Computers) AND tt.[TaskType] = 'Install' AND s.[Status] = 'Success'