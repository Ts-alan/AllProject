CREATE PROCEDURE [GetVba32VersionID]
	@Vba32Version nvarchar(64)
WITH ENCRYPTION
AS
	RETURN (SELECT [ID] FROM [Vba32Versions] WHERE [Vba32Version] = @Vba32Version)