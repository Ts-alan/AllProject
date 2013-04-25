DROP TABLE [dbo].[PoliciesGroups]
DROP TABLE [dbo].[Groups]
DROP TABLE [dbo].[GroupTypes]

--Group tables
CREATE TABLE [dbo].[GroupTypes] (
	[ID] [int] IDENTITY(1, 1) NOT NULL ,
	[GroupName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[GroupComment] nvarchar(128) COLLATE Cyrillic_General_CI_AS,
	[ParentID] int NULL,
	
	CONSTRAINT [PK_GroupTypes]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_GroupTypes_GroupName]
		UNIQUE NONCLUSTERED ([GroupName]),
	CONSTRAINT [FK_GroupTypes_GroupTypes]
		FOREIGN KEY (ParentID) REFERENCES GroupTypes([ID])					
) 
GO
-- groups to computer
CREATE TABLE [dbo].[Groups] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[ComputerID] smallint NOT NULL ,
	[GroupID] int NOT NULL,
	
	CONSTRAINT [PK_Groups]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [FK_Groups_Computers]
		FOREIGN KEY (ComputerID) REFERENCES Computers([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Groups_GroupTypes]
		FOREIGN KEY (GroupID) REFERENCES GroupTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
) 
GO
--------------------------------------------------------------------------
-- Remove group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[RemoveGroupByName]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveGroupByName]
GO

CREATE PROCEDURE [dbo].[RemoveGroupByName]
	@GroupName nvarchar(128)
WITH ENCRYPTION
AS
	DECLARE @GroupID int
	SET @GroupID = (SELECT [ID] FROM GroupTypes WHERE [GroupName] = @GroupName);

	WITH x(ID) 
	AS (
		SELECT @GroupID FROM [GroupTypes]
		UNION ALL
		SELECT gt.ID
		FROM [GroupTypes] as gt
		INNER JOIN x ON gt.ParentID = x.ID
	)
	DELETE [GroupTypes]
	FROM x
	INNER JOIN [GroupTypes] ON [GroupTypes].ID = x.ID
GO



-- Remove computer from group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[RemoveComputerFromGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveComputerFromGroup]
GO

CREATE PROCEDURE [dbo].[RemoveComputerFromGroup]
	@ComputerID smallint

WITH ENCRYPTION
AS
	DELETE FROM [Groups] WHERE ComputerID = @ComputerID
GO

-- Add computer in group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddComputerInGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddComputerInGroup]
GO

CREATE PROCEDURE [dbo].[AddComputerInGroup]
	@ComputerID smallint,
	@GroupID int

WITH ENCRYPTION
AS
	INSERT INTO [Groups] (ComputerID, GroupID)
	VALUES (@ComputerID, @GroupID)
GO

-- Move computer between groups
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[MoveComputerBetweenGroups]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[MoveComputerBetweenGroups]
GO

CREATE PROCEDURE [dbo].[MoveComputerBetweenGroups]
	@ComputerID smallint,
	@GroupID int

WITH ENCRYPTION
AS
	UPDATE [Groups]
	SET GroupID = @GroupID
	WHERE ComputerID = @ComputerID
GO


-- Get computers by Group (no subgroup computers)
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersByGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersByGroup]
GO

CREATE PROCEDURE [dbo].[GetComputersByGroup]
	@GroupID int

WITH ENCRYPTION
AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ComputersByGroupPage TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] smallint NOT NULL,
			[ComputerName] nvarchar(64) NOT NULL,
			[IPAddress] nvarchar(16) NOT NULL,
			[ControlCenter] bit NOT NULL,
			[DomainName] nvarchar(256) NULL,
			[UserLogin] nvarchar(256) NULL,
			[OSName] nvarchar(128) NULL,
			[RAM] smallint NULL,
			[CPUClock] smallint NULL,
			[RecentActive] smalldatetime NOT NULL,
			[LatestUpdate] smalldatetime NULL,
			[Vba32Version] nvarchar(256) NULL,
			[LatestInfected] smalldatetime NULL,
			[LatestMalware] nvarchar(256) NULL,
			[Vba32Integrity] bit NULL,
			[Vba32KeyValid] bit NULL,
			[Description] nvarchar(64) NULL
		)
	
		INSERT INTO @ComputersByGroupPage(
			[ID], [ComputerName], [IPAddress], [ControlCenter],
			[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
			[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description])
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description]
		FROM Groups as g
		INNER JOIN Computers AS c ON c.[ID] = g.[ComputerID]
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		WHERE g.[GroupID] = ' + CAST(@GroupID AS nvarchar(32));
	
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
			   [DomainName], [UserLogin], [OSName], [RAM], [CPUClock], 
			   [RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			   [LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description]
		FROM @ComputersByGroupPage
		ORDER BY [ComputerName] ASC'

	EXEC sp_executesql @Query

	
GO

-- Return computer count by Group (no subgroup computers)
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersByGroupCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersByGroupCount]
GO

CREATE PROCEDURE [dbo].[GetComputersByGroupCount]
	@GroupID int
WITH ENCRYPTION
AS

	SELECT COUNT (*) FROM Groups AS g			
		WHERE g.[GroupID] = @GroupID
GO

-- Return Group to computer
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetGroupToComputer]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetGroupToComputer]
GO

CREATE PROCEDURE [dbo].[GetGroupToComputer]
	@ComputerName nvarchar(64)

WITH ENCRYPTION
AS
	SELECT gt.[ID], [GroupName], [GroupComment]
	FROM [GroupTypes] AS gt
	INNER JOIN [Groups] AS g ON g.[GroupID] = gt.[ID]
	INNER JOIN [Computers] AS c ON c.[ID] = g.[ComputerID]
	WHERE c.[ComputerName] = @ComputerName
	
GO

-- Returns all Group types
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetGroupTypes]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetGroupTypes]
GO

CREATE PROCEDURE [dbo].[GetGroupTypes]
WITH ENCRYPTION
AS
	SELECT * FROM [dbo].[GroupTypes]
	ORDER BY [GroupName] ASC
GO


-- add group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddGroup]
GO

CREATE PROCEDURE [dbo].[AddGroup]
	@GroupName nvarchar(128),
	@Comment nvarchar(128) = NULL,
	@ParentID int = NULL

WITH ENCRYPTION
AS
	IF (SELECT COUNT(*) FROM [GroupTypes] WHERE GroupName = @GroupName) = 0
	BEGIN
		INSERT INTO [GroupTypes] (GroupName, GroupComment, ParentID)
		VALUES (@GroupName, @Comment, @ParentID)

		SELECT SCOPE_IDENTITY()
	END
	ELSE 
		SELECT [ID] FROM GroupTypes WHERE GroupName = @GroupName
GO

-- update group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateGroup]
GO

CREATE PROCEDURE [dbo].[UpdateGroup]
	@GroupName nvarchar(128),
	@NewGroupName nvarchar(128) = NULL,
	@NewComment nvarchar(128) = NULL,
	@NewParentID int = NULL

WITH ENCRYPTION
AS
	IF (SELECT COUNT(*) FROM [GroupTypes] WHERE GroupName = @GroupName) = 1
	BEGIN
		IF @NewGroupName IS NOT NULL
		BEGIN
			UPDATE [GroupTypes]
			SET GroupName   = @NewGroupName
			WHERE GroupName = @GroupName
		END
		IF @NewComment IS NOT NULL
		BEGIN
			UPDATE [GroupTypes]
			SET GroupComment   = @NewComment
			WHERE GroupName = @GroupName
		END
		IF @NewParentID IS NOT NULL
		BEGIN
			UPDATE [GroupTypes]
			SET ParentID   = @NewParentID
			WHERE GroupName = @GroupName
		END
	END
GO

-- Get table of computers without groups
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersWithoutGroupPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersWithoutGroupPage]
GO

CREATE PROCEDURE [dbo].[GetComputersWithoutGroupPage]
WITH ENCRYPTION
AS	
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description]
		FROM Computers as c
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		WHERE g.GroupID IS NULL	
		ORDER BY c.[ComputerName] ASC
GO

-- Get computers count without Group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersWithoutGroupPageCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersWithoutGroupPageCount]
GO

CREATE PROCEDURE [dbo].[GetComputersWithoutGroupPageCount]
WITH ENCRYPTION
AS
		SELECT COUNT(*) FROM [Computers] as c
		LEFT JOIN [Groups] as g ON c.ID = g.ComputerID
		WHERE g.GroupID IS NULL
GO

-- Get table of computers with groups
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersWithGroupPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersWithGroupPage]
GO

CREATE PROCEDURE [dbo].[GetComputersWithGroupPage]
--WITH ENCRYPTION
AS	
		SELECT	c.[ID], c.[ComputerName], g.[GroupID]
		FROM Computers as c
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
GO

------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetListGroupsCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetListGroupsCount]
GO

CREATE PROCEDURE [dbo].[GetListGroupsCount]
@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ListGroups TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[GroupID] int,			
			[GroupName] nvarchar(128) NOT NULL,
			[GroupComment] nvarchar(128) NULL,
			[ParentName] nvarchar(128) NULL
		)
	
		INSERT INTO @ListGroups([GroupID], [GroupName], [GroupComment], [ParentName])
		SELECT	gt.[ID], gt.[GroupName], gt.[GroupComment], gt_temp.[GroupName]		
		FROM GroupTypes as gt
		LEFT JOIN GroupTypes AS gt_temp ON gt_temp.[ID] = gt.[ParentID]

		DECLARE @ListGroups1 TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[GroupID] int,			
			[GroupName] nvarchar(128) NOT NULL,
			[GroupComment] nvarchar(128) NULL,
			[ParentName] nvarchar(128) NULL
		)

		INSERT INTO @ListGroups1([GroupID], [GroupName], [GroupComment], [ParentName])
		SELECT [GroupID], [GroupName], [GroupComment], [ParentName]
		FROM @ListGroups'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where		
	SET @Query = @Query + N';
		SELECT COUNT(*)
		FROM @ListGroups1'
	EXEC sp_executesql @Query
GO


-- Get table of computers with groups
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetListGroups]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetListGroups]
GO

CREATE PROCEDURE [dbo].[GetListGroups]
	@Page int,
	@RowCount int,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ListGroups TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[GroupID] int,			
			[GroupName] nvarchar(128) NOT NULL,
			[GroupComment] nvarchar(128) NULL,
			[ParentName] nvarchar(128) NULL,
			[TotalCount] int NOT NULL,
			[ActiveCount] int NOT NULL
		)
	
		INSERT INTO @ListGroups([GroupID], [GroupName], [GroupComment], [ParentName], [TotalCount], [ActiveCount])
		SELECT	gt.[ID], gt.[GroupName], gt.[GroupComment], gt_temp.[GroupName], (SELECT COUNT(gr.[ID]) FROM Groups AS gr WHERE gr.[GroupID] = gt.[ID] ),
		(SELECT COUNT(*) FROM Groups AS g
			INNER JOIN Computers AS c ON c.[ID] = g.[ComputerID]
			WHERE g.[GroupID] = gt.[ID] AND DATEDIFF(minute, [RecentActive], GetDate()) <= 120)
		FROM GroupTypes as gt
		LEFT JOIN GroupTypes AS gt_temp ON gt_temp.[ID] = gt.[ParentID]

		DECLARE @ListGroups1 TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[GroupID] int,			
			[GroupName] nvarchar(128) NOT NULL,
			[GroupComment] nvarchar(128) NULL,
			[ParentName] nvarchar(128) NULL,
			[TotalCount] int NOT NULL,
			[ActiveCount] int NOT NULL
		)

		INSERT INTO @ListGroups1([GroupID], [GroupName], [GroupComment], [ParentName], [TotalCount], [ActiveCount])
		SELECT [GroupID], [GroupName], [GroupComment], [ParentName], [TotalCount], [ActiveCount]
		FROM @ListGroups'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + ' ORDER BY ' + @OrderBy
	
	SET @Query = @Query + N';
		SELECT [GroupID], [GroupName], [GroupComment], [ParentName], [TotalCount], [ActiveCount]
		FROM @ListGroups1 WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query
GO

---POLICY
-- Get computers by Group and Policy (no subgroup computers)
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersByGroupAndPolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersByGroupAndPolicy]
GO

CREATE PROCEDURE [dbo].[GetComputersByGroupAndPolicy]
	@GroupID int = NULL,
	@PolicyID smallint = NULL	

WITH ENCRYPTION
AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ComputersByGroupAndPolicy TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] smallint NOT NULL,
			[ComputerName] nvarchar(64) NOT NULL,
			[IPAddress] nvarchar(16) NOT NULL,
			[ControlCenter] bit NOT NULL,
			[DomainName] nvarchar(256) NULL,
			[UserLogin] nvarchar(256) NULL,
			[OSName] nvarchar(128) NULL,
			[RAM] smallint NULL,
			[CPUClock] smallint NULL,
			[RecentActive] smalldatetime NOT NULL,
			[LatestUpdate] smalldatetime NULL,
			[Vba32Version] nvarchar(256) NULL,
			[LatestInfected] smalldatetime NULL,
			[LatestMalware] nvarchar(256) NULL,
			[Vba32Integrity] bit NULL,
			[Vba32KeyValid] bit NULL,
			[Description] nvarchar(64) NULL
		)
	
		INSERT INTO @ComputersByGroupAndPolicy(
			[ID], [ComputerName], [IPAddress], [ControlCenter],
			[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
			[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description])
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description]
		FROM Computers as c
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		LEFT JOIN Policies AS p ON p.[ComputerID] = c.[ID]'

		IF @GroupID IS NOT NULL
			SET @Query = @Query + ' WHERE g.[GroupID] = ' + CAST(@GroupID AS nvarchar(32));
		ELSE
			SET @Query = @Query + ' WHERE g.[GroupID] IS NULL';

		IF @PolicyID IS NOT NULL
			SET @Query = @Query + ' AND p.[PolicyID] = ' + CAST(@PolicyID AS nvarchar(32));
		ELSE
			SET @Query = @Query + ' AND p.[PolicyID] IS NULL';
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
			   [DomainName], [UserLogin], [OSName], [RAM], [CPUClock], 
			   [RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			   [LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description]
		FROM @ComputersByGroupAndPolicy
		ORDER BY [ComputerName] ASC'

	EXEC sp_executesql @Query	
GO


-- Get list of groups by computerID
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetGroupListByComputerID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetGroupListByComputerID]
GO

CREATE PROCEDURE [dbo].[GetGroupListByComputerID]
	@ComputerID smallint

WITH ENCRYPTION
AS	
		DECLARE @GroupListTemp TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] int NOT NULL,
			[GroupName] nvarchar(128) NOT NULL,
			[ParentID] int NULL,
			[GroupComment] nvarchar(128) NULL
		)

		DECLARE @GroupID int
		SET @GroupID = (SELECT gt.[ID] FROM [GroupTypes] AS gt
							INNER JOIN [Groups] AS g ON g.[GroupID] = gt.[ID]
							INNER JOIN [Computers] AS c ON c.[ID] = g.[ComputerID]
							WHERE c.[ID] = @ComputerID)

		WHILE (@GroupID IS NOT NULL)
		BEGIN
			INSERT INTO @GroupListTemp ([ID], [GroupName], [ParentID], [GroupComment])
			SELECT gt.[ID], gt.[GroupName], gt.[ParentID], gt.[GroupComment] FROM GroupTypes AS gt WHERE gt.[ID] = @GroupID
	
			SET @GroupID = 	(SELECT g.[ParentID] FROM GroupTypes AS g WHERE g.[ID] = @GroupID)
		END

		SELECT [ID], [GroupName], [ParentID], [GroupComment] FROM @GroupListTemp
GO

--------------------POLICIES
-- Clear all policies
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[ClearAllPolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[ClearAllPolicy]
GO

CREATE PROCEDURE [dbo].[ClearAllPolicy]
WITH ENCRYPTION
AS	
	DELETE FROM [Policies]
GO
