ALTER TABLE Computers
ADD [MACAddress] nvarchar(64) COLLATE Cyrillic_General_CI_AS NOT NULL
DEFAULT('0')

GO

UPDATE Computers
SET [MACAddress] = [ComputerName]
WHERE [MACAddress] = '0'

GO

ALTER TABLE Computers ADD
CONSTRAINT [U_Computers_MACAddress]
		UNIQUE NONCLUSTERED ([MACAddress])

GO

-- Create table for additional info
CREATE TABLE [dbo].[ComputerAdditionalInfo] (
	[ID] smallint IDENTITY(1, 1) NOT NULL ,
	[ComputerID] smallint NOT NULL,
	[IsControllable] bit NOT NULL,
	[IsRenamed] bit NOT NULL,
	[PreviousComputerName] nvarchar(64) COLLATE Cyrillic_General_CI_AS NULL,
	[IsMAC] bit NOT NULL,
	
	CONSTRAINT [PK_ComputerAdditionalInfo]
		PRIMARY KEY NONCLUSTERED ([ID]),
	CONSTRAINT [U_ComputerAdditionalInfo_ComputerID]
		UNIQUE NONCLUSTERED ([ComputerID]),
	CONSTRAINT [FK_ComputerAdditionalInfo_Computers]
		FOREIGN KEY ([ComputerID]) REFERENCES Computers([ID])
			ON UPDATE CASCADE ON DELETE CASCADE					
) 
GO


INSERT INTO ComputerAdditionalInfo ([ComputerID], [IsControllable], [IsRenamed], [IsMAC])
SELECT [ID], 1, 0, 0 FROM Computers
GO


-- Set MAC-address and return ComputerID
IF EXISTS (SELECT [ID] FROM dbo.sysobjects WHERE [ID] = OBJECT_ID(N'[dbo].[SetMACAddress]')
					   AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[SetMACAddress]
GO

CREATE PROCEDURE [dbo].[SetMACAddress]
	@MACAddress nvarchar(64),
	@ComputerName nvarchar(64)
WITH ENCRYPTION
AS
	DECLARE @ComputerID smallint
	SET @ComputerID = (SELECT [ID] FROM [dbo].[Computers] WHERE [MACAddress] = @ComputerName)
	
	IF @ComputerID IS NOT NULL
	BEGIN
		-- update MAC-address
		UPDATE Computers
		SET [MACAddress] = @MACAddress
		WHERE [ID] = @ComputerID

		-- set IsMAC = 1
		UPDATE ComputerAdditionalInfo
		SET [IsMAC] = 1
		WHERE [ComputerID] = @ComputerID		
	END
	ELSE
		SET @ComputerID = 0

	RETURN @ComputerID
GO




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
		EXEC @ComputerID = dbo.SetMACAddress @MACAddress, @ComputerName

	IF @ComputerID = 0
	BEGIN
	--Insert new comp
		--Check license count
		IF (SELECT COUNT([ID]) FROM [Computers]) >= @LicenseCount
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
		INSERT INTO ComputerAdditionalInfo ([ComputerID], [IsControllable], [IsRenamed], [IsMAC])
		VALUES(@@IDENTITY, 1, 0, 1)
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

		-- Recent activity time
		UPDATE [Computers]
		SET [RecentActive] = GETDATE()
		WHERE [ID] = @ComputerID
	END
	
	-- Clear Installation tasks
	EXEC dbo.[ClearInstallationTasks] @ComputerName, @IPAddress	

	IF NOT EXISTS(SELECT [ID] FROM ComputerAdditionalInfo WHERE [IsMAC] = 0)
	BEGIN		
		DROP PROCEDURE [dbo].[UpdateComputerSystemInfo]
		
		DECLARE @Query nvarchar(4000)
		SET @Query =  N'
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
					SET @OSName = ''(unknown)''
				EXEC @OSTypeID = dbo.GetOSTypeID @OSName, 1
				
				-- Checking @ControlCenter param
				IF @ControlCenter IS NULL
					SET @ControlCenter = 0

				-- Checking @IPAddress param
				IF @IPAddress IS NULL
					SET @IPAddress = ''0.0.0.0''

				-- Retrieving ComputerID
				DECLARE @ComputerID smallint
				SET @ComputerID = (SELECT [ID] FROM Computers WHERE [MACAddress] = @MACAddress)

				IF @ComputerID IS NULL		
				BEGIN
				--Insert new comp
					--Check license count
					IF (SELECT COUNT([ID]) FROM [Computers]) >= @LicenseCount
						RETURN

					-- Clearing same IPs in the database
					UPDATE [Computers]
					SET [IPAddress] = ''0.0.0.0''
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
						SET [IPAddress] = ''0.0.0.0''
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

					-- Recent activity time
					UPDATE [Computers]
					SET [RecentActive] = GETDATE()
					WHERE [ID] = @ComputerID
				END
				
				-- Clear Installation tasks
				EXEC dbo.[ClearInstallationTasks] @ComputerName, @IPAddress'		
		EXEC sp_executesql @Query

		DROP PROCEDURE [dbo].[SetMACAddress]
		
		ALTER TABLE ComputerAdditionalInfo
		DROP COLUMN [IsMAC]
	END
GO