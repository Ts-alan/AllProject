CREATE PROCEDURE [dbo].[GetDeviceClassList]		
WITH ENCRYPTION
AS
	SELECT [ID], [UID], [Class], [ClassName] FROM DeviceClass
	ORDER BY [UID] ASC