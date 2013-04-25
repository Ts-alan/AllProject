--Group tables
CREATE TABLE [dbo].[GroupTypes] (
	[ID] [int] IDENTITY(1, 1) NOT NULL ,
	[GroupName] nvarchar(128) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[GroupComment] nvarchar(128) COLLATE Cyrillic_General_CI_AS,
	
	CONSTRAINT [PK_GroupTypes]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_GroupTypes_GroupName]
		UNIQUE NONCLUSTERED ([GroupName])
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

-- policies to group
CREATE TABLE [dbo].[PoliciesGroups] (
	[ID] int IDENTITY(1, 1) NOT NULL,
	[GroupID] int NOT NULL ,
	[PolicyID] smallint NOT NULL,
	
	CONSTRAINT [PK_Policies1]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [FK_Policies_Groups]
		FOREIGN KEY (GroupID) REFERENCES GroupTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT [FK_Policies_PolicyTypes1]
		FOREIGN KEY (PolicyID) REFERENCES PolicyTypes([ID])
			ON UPDATE CASCADE ON DELETE CASCADE
) 
GO

--------------------------------------------------------------------------------------------------

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


-- Get computers by Group
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

-- Return computer count by Group
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
	SET @GroupID = (SELECT [ID] FROM GroupTypes WHERE [GroupName] = @GroupName)

	DELETE FROM [GroupTypes] WHERE ID = @GroupID
GO

-- add group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddGroup]
GO

CREATE PROCEDURE [dbo].[AddGroup]
	@GroupName nvarchar(128),
	@Comment nvarchar(128) = NULL

WITH ENCRYPTION
AS
	IF (SELECT COUNT(*) FROM [GroupTypes] WHERE GroupName = @GroupName) = 0
	BEGIN
		INSERT INTO [GroupTypes] (GroupName, GroupComment)
		VALUES (@GroupName, @Comment)

		SELECT SCOPE_IDENTITY()
	END
	ELSE 
		SELECT [ID] FROM GroupTypes WHERE GroupName = @GroupName
GO

-- rename group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[RenameGroupByName]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RenameGroupByName]
GO

CREATE PROCEDURE [dbo].[RenameGroupByName]
	@GroupName nvarchar(128),
	@NewGroupName nvarchar(128),
	@NewComment nvarchar(128) = NULL	

WITH ENCRYPTION
AS
	IF (SELECT COUNT(*) FROM [GroupTypes] WHERE GroupName = @NewGroupName) = 0
	BEGIN
		UPDATE [GroupTypes]
		SET GroupName = @NewGroupName,
		    GroupComment   = @NewComment
		WHERE GroupName = @GroupName
	END
GO

-- update comment
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateGroupComment]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateGroupComment]
GO

CREATE PROCEDURE [dbo].[UpdateGroupComment]
	@GroupName nvarchar(128),
	@NewComment nvarchar(128) = NULL	

WITH ENCRYPTION
AS
	IF (SELECT COUNT(*) FROM [GroupTypes] WHERE GroupName = @GroupName) = 1
	BEGIN
		UPDATE [GroupTypes]
		SET GroupComment   = @NewComment
		WHERE GroupName = @GroupName
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
			[PolicyName] nvarchar(128) NULL			
		)
	
		INSERT INTO @ListGroups([GroupID], [GroupName], [GroupComment], [PolicyName])
		SELECT	gt.[ID], gt.[GroupName], gt.[GroupComment], pt.[TypeName]		
		FROM GroupTypes as gt
		LEFT JOIN PoliciesGroups AS pg ON pg.[GroupID] = gt.[ID]
		LEFT JOIN PolicyTypes AS pt ON pt.[ID] = pg.[PolicyID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where
	SET @Query = @Query + N';
		SELECT COUNT(*)
		FROM @ListGroups'
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
			[PolicyName] nvarchar(128) NULL,
			[TotalCount] int NOT NULL,
			[ActiveCount] int NOT NULL
		)
	
		INSERT INTO @ListGroups([GroupID], [GroupName], [GroupComment], [PolicyName], [TotalCount], [ActiveCount])
		SELECT	gt.[ID], gt.[GroupName], gt.[GroupComment], pt.[TypeName], (SELECT COUNT(gr.[ID]) FROM Groups AS gr WHERE gr.[GroupID] = gt.[ID] ),
		(SELECT COUNT(*) FROM Groups AS g
			INNER JOIN Computers AS c ON c.[ID] = g.[ComputerID]
			WHERE g.[GroupID] = gt.[ID] AND DATEDIFF(minute, [RecentActive], GetDate()) <= 120)
		FROM GroupTypes as gt
		LEFT JOIN PoliciesGroups AS pg ON pg.[GroupID] = gt.[ID]
		LEFT JOIN PolicyTypes AS pt ON pt.[ID] = pg.[PolicyID]

		DECLARE @ListGroups1 TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[GroupID] int,			
			[GroupName] nvarchar(128) NOT NULL,
			[GroupComment] nvarchar(128) NULL,
			[PolicyName] nvarchar(128) NULL,
			[TotalCount] int NOT NULL,
			[ActiveCount] int NOT NULL
		)

		INSERT INTO @ListGroups1([GroupID], [GroupName], [GroupComment], [PolicyName], [TotalCount], [ActiveCount])
		SELECT [GroupID], [GroupName], [GroupComment], [PolicyName], [TotalCount], [ActiveCount]
		FROM @ListGroups'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + ' ORDER BY ' + @OrderBy
	
	SET @Query = @Query + N';
		SELECT [GroupID], [GroupName], [GroupComment], [PolicyName], [TotalCount], [ActiveCount]
		FROM @ListGroups1 WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query
GO


--Policy----------------------------------------------------------------------------------------------------

-- Insert computer to policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddGroupToPolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddGroupToPolicy]
GO

CREATE PROCEDURE [dbo].[AddGroupToPolicy]
	@GroupID int,
	@PolicyID smallint

WITH ENCRYPTION
AS
	INSERT INTO [PoliciesGroups](GroupID, PolicyID) VALUES (@GroupID, @PolicyID);
GO

-- Move group between policies
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[MoveGroupBetweenPolicies]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[MoveGroupBetweenPolicies]
GO

CREATE PROCEDURE [dbo].[MoveGroupBetweenPolicies]
	@GroupID int,
	@PolicyID smallint

WITH ENCRYPTION
AS
	UPDATE [PoliciesGroups]
	SET PolicyID = @PolicyID
	WHERE GroupID = @GroupID
GO


-- Remove group from policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[RemoveGroupFromPolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveGroupFromPolicy]
GO

CREATE PROCEDURE [dbo].[RemoveGroupFromPolicy]
	@GroupName nvarchar(128),
	@PolicyID smallint

WITH ENCRYPTION
AS
	DECLARE @GroupID int
	SET @GroupID = (SELECT [ID] FROM GroupTypes WHERE [GroupName] = @GroupName)

	DELETE FROM [PoliciesGroups] WHERE GroupID = @GroupID AND PolicyID = @PolicyID
GO

-- Remove group from all policies
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[RemoveGroupFromAllPolicies]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveGroupFromAllPolicies]
GO

CREATE PROCEDURE [dbo].[RemoveGroupFromAllPolicies]
	@GroupID int

WITH ENCRYPTION
AS	

	DELETE FROM [PoliciesGroups] WHERE GroupID = @GroupID
GO


-- Get groups by policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetGroupsByPolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetGroupsByPolicy]
GO

CREATE PROCEDURE [dbo].[GetGroupsByPolicy]
	@PolicyID smallint

WITH ENCRYPTION
AS
	SELECT p.[GroupID], g.[GroupName] FROM PoliciesGroups as p
	INNER JOIN GroupTypes AS g ON g.[ID] = p.[GroupID]
	WHERE p.[PolicyID] = @PolicyID
	
GO

-- Returns group count by policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetGroupsByPolicyCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetGroupsByPolicyCount]
GO

CREATE PROCEDURE [dbo].[GetGroupsByPolicyCount]
	@PolicyID smallint
WITH ENCRYPTION

AS
	SELECT COUNT (*) FROM PoliciesGroups AS p
	WHERE p.[PolicyID] = @PolicyID
GO


-- Get groups without policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetGroupsWithoutPolicyPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetGroupsWithoutPolicyPage]
GO

CREATE PROCEDURE [dbo].[GetGroupsWithoutPolicyPage]
--WITH ENCRYPTION
AS		
		SELECT	g.[ID], g.[GroupName], g.[GroupComment]
		FROM GroupTypes as g
		LEFT JOIN PoliciesGroups AS p ON g.[ID] = p.[GroupID]
		WHERE p.PolicyID IS NULL	
GO

-- Get groups count without policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetGroupsWithoutPolicyPageCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetGroupsWithoutPolicyPageCount]
GO

CREATE PROCEDURE [dbo].[GetGroupsWithoutPolicyPageCount]
--WITH ENCRYPTION
AS
	SELECT COUNT(*) FROM [GroupTypes] as g
	LEFT JOIN [PoliciesGroups] as p ON g.ID = p.GroupID
	WHERE p.PolicyID IS NULL
GO

-- Get table of computers without groups and without policies
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersWithoutGroupAndPoliciesPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersWithoutGroupAndPoliciesPage]
GO

CREATE PROCEDURE [dbo].[GetComputersWithoutGroupAndPoliciesPage]
WITH ENCRYPTION
AS	
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description]
		FROM Computers as c
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		LEFT JOIN Policies AS p ON c.[ID] = p.[ComputerID]
		WHERE g.[GroupID] IS NULL AND p.[PolicyID] IS NULL
GO

-- Get computers count without Group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersWithoutGroupAndPoliciesPageCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersWithoutGroupAndPoliciesPageCount]
GO

CREATE PROCEDURE [dbo].[GetComputersWithoutGroupAndPoliciesPageCount]
WITH ENCRYPTION
AS
		SELECT COUNT(*) FROM [Computers] as c
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		LEFT JOIN PoliciesGroups AS pg ON g.[GroupID] = pg.[GroupID]
		WHERE g.[GroupID] IS NULL AND pg.[PolicyID] IS NULL
GO

-- Get table of computers without groups by policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersWithoutGroupByPolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersWithoutGroupByPolicy]
GO

CREATE PROCEDURE [dbo].[GetComputersWithoutGroupByPolicy]
	@PolicyID smallint
WITH ENCRYPTION
AS	
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description]
		FROM Computers as c
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		LEFT JOIN Policies AS p ON c.[ID] = p.[ComputerID]
		WHERE g.[GroupID] IS NULL AND p.[PolicyID] = @PolicyID
GO

-- Get computers count without Group by policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersWithoutGroupByPolicyCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersWithoutGroupByPolicyCount]
GO

CREATE PROCEDURE [dbo].[GetComputersWithoutGroupByPolicyCount]
	@PolicyID smallint
WITH ENCRYPTION
AS
		SELECT COUNT(*) FROM [Computers] as c
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		LEFT JOIN Policies AS p ON c.[ID] = p.[ComputerID]
		WHERE g.[GroupID] IS NULL AND p.[PolicyID] = @PolicyID
GO

-- Get table of groups with policies
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetGroupsWithPolicyPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetGroupsWithPolicyPage]
GO

CREATE PROCEDURE [dbo].[GetGroupsWithPolicyPage]
--WITH ENCRYPTION
AS	
		SELECT	g.[ID], g.[GroupName], p.[PolicyID]
		FROM GroupTypes as g
		LEFT JOIN PoliciesGroups AS p ON g.[ID] = p.[GroupID]
GO

-- Get table of policies and computers without groups
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetPoliciesAndComputersWithoutGroup]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetPoliciesAndComputersWithoutGroup]
GO

CREATE PROCEDURE [dbo].[GetPoliciesAndComputersWithoutGroup]
WITH ENCRYPTION
AS	
		SELECT	c.[ID], c.[ComputerName], p.[PolicyID]
		FROM Computers as c
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		LEFT JOIN Policies AS p ON c.[ID] = p.[ComputerID]
		WHERE g.[GroupID] IS NULL
GO


-- Get policyId by groupId
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetPolicyIDByGroupID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetPolicyIDByGroupID]
GO

CREATE PROCEDURE [dbo].[GetPolicyIDByGroupID]
	@GroupID int
WITH ENCRYPTION
AS	
		SELECT	p.[PolicyID]
		FROM PoliciesGroups as p		
		WHERE p.[GroupID] = @GroupID
GO

-- Get table of computers without groups
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersWithoutGroupPageWithFilter]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersWithoutGroupPageWithFilter]
GO

CREATE PROCEDURE [dbo].[GetComputersWithoutGroupPageWithFilter]
@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @TmpList TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] smallint NOT NULL,
			[ComputerName] nvarchar(64) NOT NULL,
			[IPAddress] nvarchar(16) NOT NULL,
			[ControlCenter] bit NOT NULL,
			[DomainName] nvarchar(256) NULL,
			[UserLogin] nvarchar(256) NULL,
			[OSName] nvarchar(128) NOT NULL,
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
	
		INSERT INTO @TmpList([ID], [ComputerName], [IPAddress], [ControlCenter],
				[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
				[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
				[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description])
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description]		
		FROM Computers as c
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		WHERE g.[GroupID] IS NULL'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' AND ' + @Where
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
				[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
				[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
				[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description]
		FROM @TmpList
		ORDER BY [ComputerName] ASC'
	EXEC sp_executesql @Query
GO

-- Get computers by Group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersByGroupWithFilter]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersByGroupWithFilter]
GO

CREATE PROCEDURE [dbo].[GetComputersByGroupWithFilter]
	@GroupID int,
	@Where nvarchar(2000) = NULL
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
	
	IF @Where IS NOT NULL
		SET @Query = @Query + ' AND ' + @Where
	
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
			   [DomainName], [UserLogin], [OSName], [RAM], [CPUClock], 
			   [RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			   [LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description]
		FROM @ComputersByGroupPage
		ORDER BY [ComputerName] ASC'
	EXEC sp_executesql @Query	
GO


-- Get table of computersEx without groups
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersExWithoutGroupPageWithFilter]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersExWithoutGroupPageWithFilter]
GO

CREATE PROCEDURE [dbo].[GetComputersExWithoutGroupPageWithFilter]
@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @TmpList TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] smallint NOT NULL,
			[ComputerName] nvarchar(64) NOT NULL,
			[IPAddress] nvarchar(16) NOT NULL,
			[ControlCenter] bit NOT NULL,
			[DomainName] nvarchar(256) NULL,
			[UserLogin] nvarchar(256) NULL,
			[OSName] nvarchar(128) NOT NULL,
			[RAM] smallint NULL,
			[CPUClock] smallint NULL,
			[RecentActive] smalldatetime NOT NULL,
			[LatestUpdate] smalldatetime NULL,
			[Vba32Version] nvarchar(256) NULL,
			[LatestInfected] smalldatetime NULL,
			[LatestMalware] nvarchar(256) NULL,
			[Vba32Integrity] bit NULL,
			[Vba32KeyValid] bit NULL,
			[Description] nvarchar(64) NULL,
			[PolicyName] nvarchar(128) NULL
		)
	
		INSERT INTO @TmpList([ID], [ComputerName], [IPAddress], [ControlCenter],
				[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
				[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
				[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description], [PolicyName])
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description], pt.[TypeName]		
		FROM Computers as c
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		LEFT JOIN Groups AS g ON c.[ID] = g.[ComputerID]
		LEFT JOIN Policies AS p ON c.[ID] = p.[ComputerID]
		LEFT JOIN PolicyTypes AS pt ON pt.[ID] = p.[PolicyID]
		WHERE g.[GroupID] IS NULL'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' AND ' + @Where
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
				[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
				[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
				[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description], [PolicyName]
		FROM @TmpList
		ORDER BY [ComputerName] ASC'
	EXEC sp_executesql @Query
GO

-- Get computersEx by Group
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersExByGroupWithFilter]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersExByGroupWithFilter]
GO

CREATE PROCEDURE [dbo].[GetComputersExByGroupWithFilter]
	@GroupID int,
	@Where nvarchar(2000) = NULL
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
			[Description] nvarchar(64) NULL,
			[PolicyName] nvarchar(128) NULL
		)
	
		INSERT INTO @ComputersByGroupPage(
			[ID], [ComputerName], [IPAddress], [ControlCenter],
			[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
			[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description], [PolicyName])
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description], pt.[TypeName]
		FROM Groups as g
		INNER JOIN Computers AS c ON c.[ID] = g.[ComputerID]
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		LEFT JOIN Policies AS p ON c.[ID] = p.[ComputerID]
		LEFT JOIN PolicyTypes AS pt ON pt.[ID] = p.[PolicyID]
		WHERE g.[GroupID] = ' + CAST(@GroupID AS nvarchar(32));
	
	IF @Where IS NOT NULL
		SET @Query = @Query + ' AND ' + @Where
	
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
			   [DomainName], [UserLogin], [OSName], [RAM], [CPUClock], 
			   [RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			   [LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description], [PolicyName]
		FROM @ComputersByGroupPage
		ORDER BY [ComputerName] ASC'
	EXEC sp_executesql @Query	
GO
