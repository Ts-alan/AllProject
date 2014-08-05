CREATE PROCEDURE [OnInsertingDevice]
	@SerialNo nvarchar(256),
	@ComputerName nvarchar(64),
	@Comment nvarchar(128),
	@LicenseCount smallint
WITH ENCRYPTION
AS
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
		SET @StateID = (SELECT [ID] FROM DeviceClassMode WHERE [ModeName] = 'Undefined')

		INSERT INTO [DevicesPolicies] (ComputerID, DeviceID, DevicePolicyStateID)
		VALUES    (@ComputerID, @DeviceID, @StateID)
	END
	
	UPDATE DevicesPolicies SET [LatestInsert] = GetDate()
	WHERE [DeviceID] = @DeviceID AND [ComputerID] = @ComputerID