CREATE PROCEDURE [dbo].[GetIPAddressListForConfigure]
WITH ENCRYPTION
AS
	-- Table variable
	DECLARE @IPListTable TABLE(			
		[IPAddress] nvarchar(16) COLLATE Cyrillic_General_CI_AS NOT NULL,
		[Status] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL
	)
	
	INSERT INTO @IPListTable([IPAddress], [Status])
	SELECT DISTINCT(t.[IPAddress]), s.[Status] FROM InstallationTasks AS t	
		INNER JOIN InstallationStatus AS s ON s.[ID] = t.[StatusID]
		WHERE [IPAddress] NOT IN (SELECT [IPAddress] FROM Computers) AND (s.[Status] = 'Success' OR s.[Status] = 'Installed')

	DELETE FROM InstallationTasks WHERE [Status] = 'Installed'
	
	SELECT [IPAddress], [Status] FROM @IPListTable