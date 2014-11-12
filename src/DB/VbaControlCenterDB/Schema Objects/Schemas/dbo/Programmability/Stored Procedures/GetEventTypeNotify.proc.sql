CREATE PROCEDURE [GetEventTypeNotify]
	@EventName nvarchar(128)
WITH ENCRYPTION
AS
	SELECT
		[Notify]
	FROM
		[dbo].[EventTypes]
	WHERE
		[EventName] = @EventName