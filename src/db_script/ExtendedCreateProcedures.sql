USE vbaControlCenterDB
GO

------------ DEVICES -----------------

-- Returns Device ID by its name. Optionally insert new record to db
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDeviceID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDeviceID]
GO

CREATE PROCEDURE [GetDeviceID]
	@SerialNo nvarchar(256),
	@Type smallint,
	@Comment nvarchar(256),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such an device
		IF NOT EXISTS (SELECT [ID] FROM [Devices] WHERE [SerialNo] = @SerialNo)
			INSERT INTO [Devices](SerialNo, DeviceTypeID, Comment) VALUES (@SerialNo, 1, @Comment);
	END
	SELECT [ID] FROM [Devices] WHERE [SerialNo] = @SerialNo
END
GO

-- Returns Device by ID
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDeviceByID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDeviceByID]
GO

CREATE PROCEDURE [GetDeviceByID]
	@ID smallint
WITH ENCRYPTION
AS	
	SELECT	d.[ID], d.[SerialNo], d.[Comment]
	FROM Devices AS d
	WHERE [ID] = @ID
GO

-- Update device comment
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateDevice]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateDevice]
GO

CREATE PROCEDURE [dbo].[UpdateDevice]
	@ID smallint,
	@Comment nvarchar(256)
WITH ENCRYPTION
AS

	UPDATE [Devices]
	SET    [Comment] = @Comment
	WHERE [ID] = @ID
GO

------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeleteDevice]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteDevice]
GO

CREATE PROCEDURE [dbo].[DeleteDevice]
	@ID smallint
WITH ENCRYPTION
AS
	DELETE FROM [dbo].[Devices]
	WHERE
		[ID] = @ID
GO


IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDevicesPageCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDevicesPageCount]
GO

CREATE PROCEDURE [dbo].[GetDevicesPageCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION
AS
	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (*) FROM Devices as d					
					INNER JOIN DeviceTypes AS dt ON d.[DeviceTypeID] = dt.[ID]
					LEFT JOIN DevicesPolicies AS dp ON d.[ID] = dp.[DeviceID]
					LEFT JOIN Computers AS c ON dp.[ComputerID] = c.[ID]
					WHERE (dp.[LatestInsert] = (SELECT MAX([LatestInsert]) FROM DevicesPolicies WHERE [DeviceID] = d.[ID]) OR (0 = (SELECT COUNT([ID]) FROM DevicesPolicies WHERE [DeviceID] = d.[ID])))'
	IF @Where IS NOT NULL
		SET @Query = @Query + ' AND ' + @Where
	
	EXEC sp_executesql @Query


GO

------------ DEVICES POLICIES ---------------

	
-- Update device policies to computer
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateDevicePolicyStatusForComputer]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateDevicePolicyStatusForComputer]
GO

CREATE PROCEDURE [dbo].[UpdateDevicePolicyStatusForComputer]
	@ID smallint,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS

	DECLARE @ID1 smallint
	SET @ID1 = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)
	
	UPDATE [DevicesPolicies]
	SET    [DevicePolicyStateID] = @ID1  
	WHERE [ID] = @ID
GO


-- Get device policies to device
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetPoliciesByDevice]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetPoliciesByDevice]
GO

CREATE PROCEDURE [dbo].[GetPoliciesByDevice]
	@DeviceID smallint
WITH ENCRYPTION
AS

	 SELECT dp.[ID], dp.[ComputerID], c.[ComputerName], dp.[DeviceID], dps.[StateName],
	 d.[SerialNo], dt.[TypeName], d.[Comment], dp.[LatestInsert]
	 FROM DevicesPolicies as dp
	 INNER JOIN Computers as c ON c.[ID] = dp.[ComputerID]
	 INNER JOIN Devices as d ON dp.[DeviceID] = d.[ID]
	 INNER JOIN DevicePolicyStates as dps ON dps.[ID] = dp.[DevicePolicyStateID]
	 INNER JOIN DeviceTypes as dt ON dt.[ID] = d.[DeviceTypeID]
	 WHERE dp.DeviceID = @DeviceID
GO


-- Returns Policy ID by its name. Optionally insert new record to db
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetPolicyID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetPolicyID]
GO

CREATE PROCEDURE [GetPolicyID]
	@TypeName nvarchar(128),
	@Params ntext,
	@Comment nvarchar(128),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such an device
		IF NOT EXISTS (SELECT [ID] FROM [PolicyTypes] WHERE [TypeName] = @TypeName)
			INSERT INTO [PolicyTypes](TypeName, Params, Comment) VALUES (@TypeName, @Params, @Comment);
	END
	SELECT [ID] FROM [PolicyTypes] WHERE [TypeName] = @TypeName
END
GO


------------------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeletePolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeletePolicy]
GO

CREATE PROCEDURE [dbo].[DeletePolicy]
	@ID smallint
WITH ENCRYPTION
AS
	DELETE FROM [dbo].[PolicyTypes]
	WHERE
		[ID] = @ID
GO


-- Update policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdatePolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdatePolicy]
GO

CREATE PROCEDURE [dbo].[UpdatePolicy]
	@TypeName nvarchar(128),
	@Params ntext,
	@Comment nvarchar(128)
WITH ENCRYPTION
AS

	DECLARE @ID smallint
	SET @ID = (SELECT [ID] FROM PolicyTypes WHERE [TypeName] = @TypeName)
	
	UPDATE [PolicyTypes]
	SET    [TypeName] = @TypeName, [Params] = @Params, [Comment] = @Comment 
	WHERE [ID] = @ID
GO


-- Get policy by its name
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetPolicyByName]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetPolicyByName]
GO

CREATE PROCEDURE [dbo].[GetPolicyByName]
	@TypeName nvarchar(128)

WITH ENCRYPTION
AS
	
	SELECT [ID], [TypeName], [Params], [Comment] FROM [PolicyTypes]
	WHERE [TypeName] = @TypeName
GO

-- Insert computer to policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddComputerToPolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddComputerToPolicy]
GO

CREATE PROCEDURE [dbo].[AddComputerToPolicy]
	@ComputerName nvarchar(64),
	@PolicyID smallint

WITH ENCRYPTION
AS
	DECLARE @ComputerID smallint
	SET @ComputerID = (SELECT [ID] FROM Computers WHERE [ComputerName] = @ComputerName)

	INSERT INTO [Policies](ComputerID, PolicyID) VALUES (@ComputerID, @PolicyID);
GO

-- Add device policies to computer
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[AddDevicePolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddDevicePolicy]
GO

CREATE PROCEDURE [dbo].[AddDevicePolicy]
	@ComputerID smallint,
	@DeviceID smallint,
	@StateName nvarchar(64)
WITH ENCRYPTION
AS


	DECLARE @ID smallint
	SET @ID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = @StateName)
	
	INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
	VALUES    (@ComputerID, @DeviceID, @ID)
GO

-- Remove computer from policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[RemoveComputerFromPolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveComputerFromPolicy]
GO

CREATE PROCEDURE [dbo].[RemoveComputerFromPolicy]
	@ComputerName nvarchar(64),
	@PolicyID smallint

WITH ENCRYPTION
AS
	DECLARE @ComputerID smallint
	SET @ComputerID = (SELECT [ID] FROM Computers WHERE [ComputerName] = @ComputerName)

	DELETE FROM [Policies] WHERE ComputerID = @ComputerID AND PolicyID = @PolicyID
GO

-- Remove computer from all policies
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[RemoveComputerFromAllPolicies]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[RemoveComputerFromAllPolicies]
GO

CREATE PROCEDURE [dbo].[RemoveComputerFromAllPolicies]
	@ComputerName nvarchar(64)

WITH ENCRYPTION
AS
	DECLARE @ComputerID smallint
	SET @ComputerID = (SELECT [ID] FROM Computers WHERE [ComputerName] = @ComputerName)

	DELETE FROM [Policies] WHERE ComputerID = @ComputerID
GO

-- Get computers by policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersByPolicy]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersByPolicy]
GO

CREATE PROCEDURE [dbo].[GetComputersByPolicy]
	@PolicyID smallint

WITH ENCRYPTION
AS
	SELECT p.[ComputerID], c.[ComputerName] FROM Policies as p
	INNER JOIN Computers AS c ON c.[ID] = p.[ComputerID]
	WHERE p.[PolicyID] = @PolicyID
	
GO

-- Get computers by policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersByPolicyPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersByPolicyPage]
GO

CREATE PROCEDURE [dbo].[GetComputersByPolicyPage]
	@Page smallint,
	@RowCount smallint,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL

WITH ENCRYPTION
AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ComputersByPolicyPage TABLE(
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
	
		INSERT INTO @ComputersByPolicyPage(
			[ID], [ComputerName], [IPAddress], [ControlCenter],
			[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
			[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description])
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description]
		FROM Policies as p
		INNER JOIN Computers AS c ON c.[ID] = p.[ComputerID]
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
			   [DomainName], [UserLogin], [OSName], [RAM], [CPUClock], 
			   [RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			   [LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description]
		FROM @ComputersByPolicyPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query

	
GO

-- Returns computer count by policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersByPolicyCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersByPolicyCount]
GO

CREATE PROCEDURE [dbo].[GetComputersByPolicyCount]
	@Where nvarchar(2000) = NULL
WITH ENCRYPTION

AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'SELECT COUNT (c.[ID]) FROM Policies AS p
			INNER JOIN Computers AS c ON c.[ID] = p.[ComputerID]
			INNER JOIN PolicyTypes AS pt ON  pt.[ID] = p.[PolicyID] 
'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	EXEC sp_executesql @Query

GO


-- Get computers without policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersWithoutPolicyPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersWithoutPolicyPage]
GO

CREATE PROCEDURE [dbo].[GetComputersWithoutPolicyPage]
	@Page smallint,
	@RowCount smallint,
	@OrderBy nvarchar(64) = NULL
--WITH ENCRYPTION
AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @ComputersWithoutPolicyPage TABLE(
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
	
		INSERT INTO @ComputersWithoutPolicyPage(
			[ID], [ComputerName], [IPAddress], [ControlCenter],
			[DomainName], [UserLogin], [OSName], [RAM], [CPUClock],
			[RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			[LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description])
		SELECT	c.[ID], c.[ComputerName], c.[IPAddress], c.[ControlCenter],
				c.[DomainName], c.[UserLogin], o.[OSName], c.[RAM], c.[CPUClock],
				c.[RecentActive], c.[LatestUpdate], c.[Vba32Version], c.[LatestInfected],
				c.[LatestMalware], c.[Vba32Integrity], c.[Vba32KeyValid], c.[Description]
		FROM Computers as c
		LEFT JOIN Policies AS p ON c.[ID] = p.[ComputerID]
		INNER JOIN OSTypes AS o ON c.[OSTypeID] = o.[ID]
		WHERE p.PolicyID IS NULL'
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [ComputerName], [IPAddress], [ControlCenter],
			   [DomainName], [UserLogin], [OSName], [RAM], [CPUClock], 
			   [RecentActive], [LatestUpdate], [Vba32Version], [LatestInfected],
			   [LatestMalware], [Vba32Integrity], [Vba32KeyValid], [Description]
		FROM @ComputersWithoutPolicyPage WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query
GO

-- Get computers count without policy
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetComputersWithoutPolicyPageCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetComputersWithoutPolicyPageCount]
GO

CREATE PROCEDURE [dbo].[GetComputersWithoutPolicyPageCount]
--WITH ENCRYPTION
AS
	SELECT COUNT(*) FROM [Computers] as c
	LEFT JOIN [Policies] as p ON c.ID = p.ComputerID
	WHERE p.PolicyID IS NULL
GO

-- Returns policies to computer
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetPoliciesToComputer]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetPoliciesToComputer]
GO

CREATE PROCEDURE [dbo].[GetPoliciesToComputer]
	@ComputerName nvarchar(64)

WITH ENCRYPTION
AS
	SELECT pt.[ID], [TypeName], [Params], [Comment]
	FROM [PolicyTypes] AS pt
	INNER JOIN [Policies] AS p ON p.[PolicyID] = pt.[ID]
	INNER JOIN [Computers] AS c ON c.[ID] = p.[ComputerID]
	WHERE c.[ComputerName] = @ComputerName
	
GO

-- Returns all policy types
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetPolicyTypes]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetPolicyTypes]
GO

CREATE PROCEDURE [dbo].[GetPolicyTypes]
WITH ENCRYPTION
AS
	SELECT * FROM [dbo].[PolicyTypes]
GO

-- Get device policy by id
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDevicePolicyByID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDevicePolicyByID]
GO

CREATE PROCEDURE [dbo].[GetDevicePolicyByID]
	@ID int
WITH ENCRYPTION
AS

	 SELECT dp.[ID], dp.[ComputerID], c.[ComputerName], dp.[DeviceID], dps.[StateName],
	 d.[SerialNo], dt.[TypeName], d.[Comment], dp.[LatestInsert]
	 FROM DevicesPolicies as dp
	 INNER JOIN Computers as c ON c.[ID] = dp.[ComputerID]
	 INNER JOIN Devices as d ON dp.[DeviceID] = d.[ID]
	 INNER JOIN DevicePolicyStates as dps ON dps.[ID] = dp.[DevicePolicyStateID]
	 INNER JOIN DeviceTypes as dt ON dt.[ID] = d.[DeviceTypeID]
	 WHERE dp.[ID] = @ID
GO

-- Delete device policy by id
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[DeleteDevicePolicyByID]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteDevicePolicyByID]
GO

CREATE PROCEDURE [dbo].[DeleteDevicePolicyByID]
	@ID int
WITH ENCRYPTION
AS

	 DELETE	 FROM DevicesPolicies WHERE [ID] = @ID
GO


-- add devicepolicy to computer
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[OnInsertingDevice]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[OnInsertingDevice]
GO

CREATE PROCEDURE [OnInsertingDevice]
	@SerialNo nvarchar(256),
	@ComputerName nvarchar(64),
	@Comment nvarchar(128)
WITH ENCRYPTION
AS
BEGIN
	-- get computer id
	DECLARE @ComputerID smallint
	SET @ComputerID = (SELECT [ID] FROM [Computers] WHERE [ComputerName] = @ComputerName)

	--device not exist
	IF NOT EXISTS ((SELECT [ID] FROM [Devices] WHERE [SerialNo] = @SerialNo))
	BEGIN
		--insert device
		INSERT INTO [Devices](SerialNo, DeviceTypeID, Comment) VALUES (@SerialNo, 1, @Comment);
	END
	
	--get device id
	DECLARE @DeviceID smallint
	SET @DeviceID = (SELECT [ID] FROM [Devices] WHERE [SerialNo] = @SerialNo)

	--not exesit policy to this comp and device
	IF NOT EXISTS ((SELECT [ID] FROM [DevicesPolicies] 
					WHERE [ComputerID] = @ComputerID AND
						  [DeviceID] = @DeviceID))
	BEGIN
		DECLARE @StateID smallint
		SET @StateID = (SELECT [ID] FROM DevicePolicyStates WHERE [StateName] = 'undefined')

		INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
		VALUES    (@ComputerID, @DeviceID, @StateID)
	END
	
	UPDATE DevicesPolicies SET [LatestInsert] = GetDate()
	WHERE [DeviceID] = @DeviceID AND [ComputerID] = @ComputerID

END
GO

IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDevicesPolicyPage]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDevicesPolicyPage]
GO

CREATE PROCEDURE [dbo].[GetDevicesPolicyPage]
	@Page smallint,
	@RowCount smallint,
	@OrderBy nvarchar(64) = NULL,
	@Where nvarchar(2000) = NULL
AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		DECLARE @UnknownDevicesPolicy TABLE(
			[RecID] smallint IDENTITY(1, 1) NOT NULL,
			[ID] int NOT NULL,
			[ComputerID] smallint NOT NULL,
			[ComputerName] nvarchar(64) NOT NULL,
			[DeviceID] smallint NOT NULL,
			[StateName] nvarchar(64) NOT NULL,
			[SerialNo] nvarchar(256) NOT NULL ,
			[TypeName] nvarchar(256) NOT NULL,
			[Comment] nvarchar(128) NULL,
			[LatestInsert] smalldatetime NULL
		)
	
		INSERT INTO @UnknownDevicesPolicy([ID], [ComputerID], [ComputerName], [DeviceID], 
			[StateName],[SerialNo], [TypeName], [Comment],[LatestInsert])
		SELECT dp.[ID], dp.[ComputerID], c.[ComputerName], dp.[DeviceID], dps.[StateName],
		 d.[SerialNo], dt.[TypeName], d.[Comment], dp.[LatestInsert]
		 FROM DevicesPolicies as dp
		 INNER JOIN Computers as c ON c.[ID] = dp.[ComputerID]
		 INNER JOIN Devices as d ON dp.[DeviceID] = d.[ID]
		 INNER JOIN DevicePolicyStates as dps ON dps.[ID] = dp.[DevicePolicyStateID]
		 INNER JOIN DeviceTypes as dt ON dt.[ID] = d.[DeviceTypeID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where
	IF @OrderBy IS NOT NULL
		SET @Query = @Query + N' ORDER BY ' + @OrderBy
	SET @Query = @Query + N';
		SELECT [ID], [ComputerID], [ComputerName], [DeviceID], [StateName],
			[SerialNo], [TypeName], [Comment],[LatestInsert]
		FROM @UnknownDevicesPolicy WHERE [RecID] BETWEEN (' +
			+ STR(@RowCount) + N' * (' + STR(@Page) + N' - 1) + 1) AND (' +
			+ STR(@RowCount) + N' * ' + STR(@Page) + N' )'
	EXEC sp_executesql @Query

GO


IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[GetDevicesPolicyPageCount]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetDevicesPolicyPageCount]
GO

CREATE PROCEDURE [dbo].[GetDevicesPolicyPageCount]
	@Where nvarchar(2000) = NULL
AS

	DECLARE @Query nvarchar(4000)
	SET @Query =  N'
		SELECT COUNT(*)
		 FROM DevicesPolicies as dp
		 INNER JOIN Computers as c ON c.[ID] = dp.[ComputerID]
		 INNER JOIN Devices as d ON dp.[DeviceID] = d.[ID]
		 INNER JOIN DevicePolicyStates as dps ON dps.[ID] = dp.[DevicePolicyStateID]
		 INNER JOIN DeviceTypes as dt ON dt.[ID] = d.[DeviceTypeID]'
	IF @Where IS NOT NULL
		SET @Query = @Query + N' WHERE ' + @Where	
	EXEC sp_executesql @Query

GO