CREATE PROCEDURE [DeleteOldEvents]
	@Date datetime
WITH ENCRYPTION
AS
	SET ROWCOUNT 10000 

	WHILE (667 = 667)
	BEGIN
		DELETE [Events] FROM [EventTypes] INNER JOIN [Events] ON [EventTypes].[ID] = [Events].[EventID]
			WHERE ([EventTypes].[NoDelete] = 0) AND ([Events].[EventTime] < @Date)
        
		IF @@ROWCOUNT < 10000
		BREAK
	END

	SET ROWCOUNT 0