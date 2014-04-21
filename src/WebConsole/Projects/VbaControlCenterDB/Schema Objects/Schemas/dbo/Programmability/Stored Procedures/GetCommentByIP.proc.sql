CREATE PROCEDURE [GetCommentByIP]
	@IP nvarchar(16)
WITH ENCRYPTION
AS
	SELECT [Comment] FROM [ScanningComments] WHERE [IPAddress] = @IP