CREATE PROCEDURE [dbo].[GetEventsCountByComputer]
	@ComputerName nvarchar(64),
	@EventName nvarchar(128),
	@EventTime datetime
WITH ENCRYPTION
AS
	DECLARE @Now datetime
	SET @Now = GETDATE()
	SELECT COUNT([Events].[ID])
	FROM [dbo].[Events]
		INNER JOIN [dbo].[Computers] ON [Computers].[ID] = [Events].[ComputerID]
		INNER JOIN [dbo].[EventTypes] ON [EventTypes].[ID] = [Events].[EventID]
	WHERE [ComputerName] = @ComputerName AND [EventName] = @EventName AND [EventTime] BETWEEN @EventTime AND @Now;