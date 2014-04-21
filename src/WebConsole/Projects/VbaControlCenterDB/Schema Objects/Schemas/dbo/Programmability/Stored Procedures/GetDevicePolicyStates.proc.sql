CREATE PROCEDURE [GetDevicePolicyStates]
WITH ENCRYPTION
AS
	SELECT [StateName] FROM DevicePolicyStates
	ORDER BY [StateName] ASC