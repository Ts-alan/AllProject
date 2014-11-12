CREATE PROCEDURE [GetListInstallationStatus]
WITH ENCRYPTION
AS
	SELECT [Status] FROM InstallationStatus