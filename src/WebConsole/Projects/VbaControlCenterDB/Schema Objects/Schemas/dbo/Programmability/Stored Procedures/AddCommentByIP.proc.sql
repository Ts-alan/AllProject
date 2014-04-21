CREATE PROCEDURE [AddCommentByIP]
	@IP nvarchar(16),
	@Comment nvarchar(128)
WITH ENCRYPTION
AS
BEGIN
	DELETE FROM [ScanningComments] WHERE [IPAddress]=@IP

	INSERT INTO [ScanningComments] ([IPAddress], [Comment])
	VALUES(@IP, @Comment)
END