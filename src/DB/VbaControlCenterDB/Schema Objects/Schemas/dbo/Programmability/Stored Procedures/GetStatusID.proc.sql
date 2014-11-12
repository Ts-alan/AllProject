CREATE PROCEDURE [GetStatusID]
	@Status nvarchar(64)
WITH ENCRYPTION
AS
	RETURN (SELECT [ID] FROM [InstallationStatus] WHERE [Status] = @Status)