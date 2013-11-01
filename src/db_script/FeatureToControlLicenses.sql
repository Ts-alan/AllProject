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
	@LicenseCount smallint,
	@MACAddress	nvarchar(64)
WITH ENCRYPTION
AS
	-- Retrieving OSID
	DECLARE @OSTypeID smallint	
	IF @OSName IS NULL
		SET @OSName = '(unknown)'    
    EXEC @OSTypeID = dbo.GetOSTypeID @OSName, 1
	
	-- Checking @ControlCenter param
	IF @ControlCenter IS NULL
		SET @ControlCenter = 0

	-- Checking @IPAddress param
	IF @IPAddress IS NULL
		SET @IPAddress = '0.0.0.0'

	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	SET @ComputerID = (SELECT [ID] FROM Computers WHERE [MACAddress] = @MACAddress)

	IF @ComputerID IS NULL		
	BEGIN
	--Insert new comp
		--Check license count
		IF (SELECT COUNT([ID]) FROM [Computers]) >= @LicenseCount
			RETURN

		IF EXISTS(SELECT [ID] FROM [Computers] WHERE [ComputerName] = @ComputerName)
			RETURN

		-- Clearing same IPs in the database
		UPDATE [Computers]
		SET [IPAddress] = '0.0.0.0'
		WHERE [IPAddress] = @IPAddress
		
		-- New computer registration
		INSERT INTO [Computers](MACAddress, ComputerName, IPAddress, ControlCenter, DomainName, UserLogin,
								OSTypeID, RAM, CPUClock, RecentActive, Vba32Version, Vba32Integrity, Vba32KeyValid)
		VALUES (@MACAddress, @ComputerName, @IPAddress, @ControlCenter, @DomainName, @UserLogin,
				@OSTypeID, @RAM, @CPUClock, GETDATE(), @Vba32Version, @Vba32Integrity, @Vba32KeyValid)

		-- Additional info
		INSERT INTO ComputerAdditionalInfo ([ComputerID], [IsControllable], [IsRenamed])
		VALUES(@@IDENTITY, 1, 0)
	END
	ELSE BEGIN
	-- Update info
		--Update ComputerName
		DECLARE @CurrentComputerName nvarchar(64)
		SET @CurrentComputerName = (SELECT [ComputerName] FROM Computers WHERE [ID] = @ComputerID)
		IF @CurrentComputerName != @ComputerName
		BEGIN
			UPDATE ComputerAdditionalInfo
			SET [PreviousComputerName] = @CurrentComputerName,
				[IsRenamed] = 1
			WHERE [ComputerID] = @ComputerID

			UPDATE Computers
			SET [ComputerName] = @ComputerName
			WHERE [ID] = @ComputerID
		END
		
		-- Update IPAddress
		IF NOT EXISTS(SELECT [ID] FROM Computers WHERE [ID] = @ComputerID AND [IPAddress] = @IPAddress)
		BEGIN
			UPDATE [Computers]
			SET [IPAddress] = '0.0.0.0'
			WHERE [IPAddress] = @IPAddress

			UPDATE [Computers]
			SET [IPAddress] = @IPAddress
			WHERE [ID] = @ComputerID
		END

		-- Others
		IF @DomainName IS NOT NULL
			UPDATE [Computers]
			SET DomainName = @DomainName
			WHERE [ID] = @ComputerID
		IF @UserLogin IS NOT NULL
			UPDATE [Computers]
			SET UserLogin = @UserLogin
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
		
		UPDATE [Computers]
		SET ControlCenter = @ControlCenter
		WHERE [ID] = @ComputerID
		
		UPDATE [Computers]
		SET OSTypeID = @OSTypeID
		WHERE [ID] = @ComputerID

		-- Set isControlable in TRUE
		UPDATE ComputerAdditionalInfo
		SET IsControllable = 1
		WHERE [ComputerID] = @ComputerID

		-- Recent activity time
		UPDATE [Computers]
		SET [RecentActive] = GETDATE()
		WHERE [ID] = @ComputerID
	END
	
	-- Clear Installation tasks
	EXEC dbo.[ClearInstallationTasks] @ComputerName, @IPAddress	
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