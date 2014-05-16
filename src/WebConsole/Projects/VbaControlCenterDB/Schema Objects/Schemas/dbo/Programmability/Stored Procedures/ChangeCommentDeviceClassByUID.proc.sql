CREATE PROCEDURE [dbo].[ChangeCommentDeviceClassByUID]
	@UID nvarchar(38),
	@Comment nvarchar(128) = NULL
WITH ENCRYPTION
AS
	UPDATE DeviceClass
	SET [Class] = @Comment
	WHERE [UID] = @UID