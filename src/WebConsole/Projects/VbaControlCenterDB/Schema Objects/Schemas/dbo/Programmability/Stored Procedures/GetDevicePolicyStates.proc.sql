CREATE PROCEDURE [GetDevicePolicyStates]
WITH ENCRYPTION
AS
	SELECT [ModeName] FROM DeviceClassMode
	ORDER BY [ModeName] ASC