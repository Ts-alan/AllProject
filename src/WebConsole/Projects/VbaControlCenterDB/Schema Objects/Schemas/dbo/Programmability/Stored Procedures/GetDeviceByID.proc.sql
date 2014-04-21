CREATE PROCEDURE [GetDeviceByID]
	@ID smallint
WITH ENCRYPTION
AS	
	SELECT	d.[ID], d.[SerialNo], d.[Comment]
	FROM Devices AS d
	WHERE [ID] = @ID