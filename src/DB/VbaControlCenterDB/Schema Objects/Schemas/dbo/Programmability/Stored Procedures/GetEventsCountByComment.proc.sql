CREATE PROCEDURE [dbo].[GetEventsCountByComment]
	@Comment nvarchar(64),
	@EventName nvarchar(128),
	@EventTime datetime
WITH ENCRYPTION
AS
	DECLARE @Now datetime
	SET @Now = GETDATE()
	SELECT [ComputerName], COUNT([Events].[ID])
	FROM [dbo].[Events]
		INNER JOIN [dbo].[EventTypes] ON [EventTypes].[ID] = [Events].[EventID]
		INNER JOIN [dbo].[Computers] ON [Computers].[ID] = [Events].[ComputerID]
	WHERE [Comment] = @Comment AND [EventName] = @EventName AND [EventTime] BETWEEN @EventTime AND @Now
	GROUP BY [ComputerName];