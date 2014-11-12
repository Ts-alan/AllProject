CREATE PROCEDURE [dbo].[GetDeviceBySN]
	@SerialNo nvarchar(1024)
WITH ENCRYPTION
AS	
	SELECT	d.[ID], d.[SerialNo], d.[Comment]
	FROM Devices AS d
	WHERE SerialNo = @SerialNo