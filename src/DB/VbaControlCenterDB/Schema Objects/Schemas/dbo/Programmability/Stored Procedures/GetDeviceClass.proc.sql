CREATE PROCEDURE [dbo].[GetDeviceClass]
	@UID nvarchar(38)
WITH ENCRYPTION
AS
	SELECT [ID], [UID], [Class], [ClassName] FROM DeviceClass
	WHERE [UID] = @UID