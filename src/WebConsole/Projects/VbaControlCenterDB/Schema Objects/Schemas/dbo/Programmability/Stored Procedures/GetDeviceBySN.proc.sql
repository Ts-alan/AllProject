CREATE PROCEDURE [dbo].[GetDeviceBySN]
	@SerialNo nvarchar(256)
WITH ENCRYPTION
AS	
	SELECT	d.[ID], d.[SerialNo], d.[Comment]
	FROM Devices AS d
	WHERE SerialNo = @SerialNo