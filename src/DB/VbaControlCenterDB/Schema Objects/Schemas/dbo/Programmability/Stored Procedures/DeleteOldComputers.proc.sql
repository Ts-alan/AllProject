CREATE PROCEDURE [DeleteOldComputers]
	@Date smalldatetime = NULL
WITH ENCRYPTION
AS
	DELETE FROM Computers
		WHERE [IPAddress] = N'0.0.0.0'
		
    IF @Date IS NOT NULL
		DELETE FROM Computers
		WHERE [RecentActive] < @Date