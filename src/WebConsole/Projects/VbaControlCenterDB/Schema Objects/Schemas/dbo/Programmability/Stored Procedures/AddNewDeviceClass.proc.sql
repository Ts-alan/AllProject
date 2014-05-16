CREATE PROCEDURE [dbo].[AddNewDeviceClass]
	@UID nvarchar(38),
	@Class nvarchar(128) = NULL,
	@ClassName nvarchar(128) = NULL
WITH ENCRYPTION
AS
	INSERT INTO DeviceClass ([UID], [Class], [ClassName])
	VALUES (@UID, @Class, @ClassName)

	SELECT @@IDENTITY