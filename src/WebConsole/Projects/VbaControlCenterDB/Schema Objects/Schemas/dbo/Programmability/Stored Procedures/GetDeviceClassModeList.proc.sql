CREATE PROCEDURE [dbo].[GetDeviceClassModeList]	
WITH ENCRYPTION
AS
	SELECT [ID], [ModeName] FROM DeviceClassMode