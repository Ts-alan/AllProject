CREATE PROCEDURE [GetDeviceID]
	@SerialNo nvarchar(1024),
	@Type smallint,
	@Comment nvarchar(256),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such an device
		IF NOT EXISTS (SELECT [ID] FROM [Devices] WHERE [SerialNo] = @SerialNo)
			INSERT INTO [Devices](SerialNo, DeviceTypeID, Comment) VALUES (@SerialNo, 1, @Comment);
	END
	SELECT [ID] FROM [Devices] WHERE [SerialNo] = @SerialNo