CREATE PROCEDURE [DeleteCommentByIP]
	@IP nvarchar(16)
WITH ENCRYPTION
AS
	DELETE FROM [ScanningComments] WHERE [IPAddress] = @IP