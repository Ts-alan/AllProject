USE vbaControlCenterDB
GO

-----------------------------------------
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[UpdateComputerSystemInfo]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateComputerSystemInfo]
GO

CREATE PROCEDURE [UpdateComputerSystemInfo]
	@ComputerName nvarchar(64),
	@IPAddress nvarchar(16) = NULL,
	@DomainName nvarchar(256) = NULL,
	@UserLogin nvarchar(256) = NULL,
	@OSName nvarchar(128) = NULL,
	@RAM smallint = NULL,
	@CPUClock smallint = NULL,
	@Vba32Version nvarchar(256) = NULL,
	@Vba32Integrity bit = NULL,
	@Vba32KeyValid bit = NULL,
	@ControlCenter bit = NULL,
	@LicenseCount smallint	
WITH ENCRYPTION
AS
	-- Retrieving OSID
	DECLARE @OSTypeID smallint
	SET @OSTypeID = NULL
	IF @OSName IS NOT NULL
		EXEC @OSTypeID = dbo.GetOSTypeID @OSName, 1

	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID = 0
	BEGIN
		IF (SELECT COUNT([ID]) FROM [Computers]) >= @LicenseCount
			RETURN

		IF @IPAddress IS NOT NULL
			-- Clearing same IPs in the database
			UPDATE [Computers]
			SET [IPAddress] = '0.0.0.0'
			WHERE [IPAddress] = @IPAddress
		ELSE
			SET @IPAddress = '0.0.0.0'
		-- Checking @ControlCenter param
		IF @ControlCenter IS NULL
			SET @ControlCenter = 0
		-- Checking @OSName param
		IF @OSName IS NULL
			BEGIN
				SET @OSName = '(unknown)'
				EXEC @OSTypeID = dbo.GetOSTypeID @OSName, 1
			END
		-- New computer registration
		INSERT INTO [Computers](ComputerName,
								IPAddress,
								ControlCenter,
								DomainName,
								UserLogin,
								OSTypeID,
								RAM,
								CPUClock,
								RecentActive,
								Vba32Version,
								Vba32Integrity,
								Vba32KeyValid)
		VALUES (@ComputerName,
				@IPAddress,
				@ControlCenter,
				@DomainName,
				@UserLogin,
				@OSTypeID,
				@RAM,
				@CPUClock,
				GETDATE(),
				@Vba32Version,
				@Vba32Integrity,
				@Vba32KeyValid)
		RETURN
	END
	
	-- Updating particular fields
	IF @IPAddress IS NOT NULL
		BEGIN
			-- Clearing same IPs in the database
			UPDATE [Computers]
			SET [IPAddress] = '0.0.0.0'
			WHERE [IPAddress] = @IPAddress
			-- Updating value for current computer
			UPDATE [Computers]
			SET IPAddress = @IPAddress
			WHERE [ID] = @ComputerID
		END
	IF @DomainName IS NOT NULL
		UPDATE [Computers]
		SET DomainName = @DomainName
		WHERE [ID] = @ComputerID
	IF @UserLogin IS NOT NULL
		UPDATE [Computers]
		SET UserLogin = @UserLogin
		WHERE [ID] = @ComputerID
	IF @OSTypeID IS NOT NULL
		UPDATE [Computers]
		SET OSTypeID = @OSTypeID
		WHERE [ID] = @ComputerID
	IF @RAM IS NOT NULL
		UPDATE [Computers]
		SET RAM = @RAM
		WHERE [ID] = @ComputerID
	IF @CPUClock IS NOT NULL
		UPDATE [Computers]
		SET CPUClock = @CPUClock
		WHERE [ID] = @ComputerID
	IF @Vba32Version IS NOT NULL
		UPDATE [Computers]
		SET Vba32Version = @Vba32Version
		WHERE [ID] = @ComputerID
	IF @Vba32Integrity IS NOT NULL
		UPDATE [Computers]
		SET Vba32Integrity = @Vba32Integrity
		WHERE [ID] = @ComputerID
	IF @Vba32KeyValid IS NOT NULL
		UPDATE [Computers]
		SET Vba32KeyValid = @Vba32KeyValid
		WHERE [ID] = @ComputerID
	IF @ControlCenter IS NOT NULL
		UPDATE [Computers]
		SET ControlCenter = @ControlCenter
		WHERE [ID] = @ComputerID

	-- Clear Installation tasks
	EXEC dbo.[ClearInstallationTasks] @ComputerName, @IPAddress

	-- Recent activity time
	UPDATE [Computers]
	SET [RecentActive] = GETDATE()
	WHERE [ID] = @ComputerID
GO
--------------------------------------------------------------------------------------------------

-- add devicepolicy to computer
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[OnInsertingDevice]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[OnInsertingDevice]
GO

CREATE PROCEDURE [OnInsertingDevice]
	@SerialNo nvarchar(256),
	@ComputerName nvarchar(64),
	@Comment nvarchar(128),
	@LicenseCount smallint
WITH ENCRYPTION
AS
BEGIN
	-- get computer id
	DECLARE @ComputerID smallint
	SET @ComputerID = (SELECT [ID] FROM [Computers] WHERE [ComputerName] = @ComputerName)

	IF NOT EXISTS (SELECT [ID] FROM [DevicesPolicies] WHERE [ComputerID] = @ComputerID)
	BEGIN
		IF (SELECT COUNT(DISTINCT(ComputerID)) FROM [DevicesPolicies]) >= @LicenseCount
			RETURN		
	END

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