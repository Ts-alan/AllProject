DROP TABLE [dbo].[Processes]
DROP TABLE [dbo].[ProcessNames]


-- Process-related tables
CREATE TABLE [dbo].[Processes] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerID] smallint NOT NULL,
	[ProcessName] nvarchar(260) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[MemorySize] int NULL,
	[LastDate] datetime NOT NULL,
	CONSTRAINT [PK_Processes]
		PRIMARY KEY NONCLUSTERED ([ID]),	
	CONSTRAINT [FK_Processes_Computers]
		FOREIGN KEY (ComputerID) REFERENCES Computers([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
)

GO

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeleteProcessInfo]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteProcessInfo]
GO

CREATE PROCEDURE [DeleteProcessInfo]
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID IS NULL
	BEGIN
		RAISERROR(N'Unable to find computer %s', 16, 1, @ComputerName)
		RETURN
	END
	
	DELETE FROM [Processes] WHERE [ComputerID] = @ComputerID
GO

-----------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateProcessInfo]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateProcessInfo]
GO

CREATE PROCEDURE [UpdateProcessInfo]
	@ComputerName nvarchar(64),
	@ProcessName nvarchar(260),
	@MemorySize int,
	@Date datetime
WITH ENCRYPTION
AS
	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID = 0
	BEGIN
		RAISERROR(N'Unable to find computer %s', 16, 1, @ComputerName)
		RETURN
	END
	-- Insert
	INSERT INTO [Processes](ComputerID, ProcessName, MemorySize, LastDate)
		VALUES(@ComputerID, @ProcessName, @MemorySize, @Date)	
GO


-- Returns count of Processes that match the criteria
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetProcessesCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetProcessesCount]
GO

CREATE PROCEDURE [dbo].[GetProcessesCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (p.[ID]) FROM Processes AS p
					INNER JOIN Computers AS c ON p.[ComputerID] = c.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query
GO


-- Returns a page from Processes table
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetProcessesPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetProcessesPage]
GO

CREATE PROCEDURE [dbo].[GetProcessesPage]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		-- Table variable - for paging
		DECLARE @ProcessesPage TABLE(
			[RecID] int IDENTITY(1, 1) NOT NULL,
			[ComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[ProcessName] nvarchar(260) COLLATE Cyrillic_General_CI_AS NOT NULL,
			[MemorySize] int NULL,
			[LastDate] datetime NOT NULL
		)
	
		INSERT INTO @ProcessesPage(
			[ComputerName], [ProcessName], [MemorySize], [LastDate])
		SELECT
			c.[ComputerName], p.[ProcessName], p.[MemorySize], p.[LastDate]
		FROM Processes AS p
		INNER JOIN Computers AS c ON p.[ComputerID] = c.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ComputerName], [ProcessName], [MemorySize], [LastDate]
		FROM @ProcessesPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query
GO
