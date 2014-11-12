CREATE PROCEDURE [dbo].[GetEventsCountByName]
	@EventName nvarchar(128),
	@EventTime datetime
WITH ENCRYPTION
AS
	DECLARE @Now datetime
	SET @Now = GETDATE()
	SELECT COUNT([Events].[ID])
	FROM [dbo].[Events]
		INNER JOIN [dbo].[EventTypes] ON [EventTypes].[ID] = [Events].[EventID]
	WHERE [EventName] = @EventName AND [EventTime] BETWEEN @EventTime AND @Now;