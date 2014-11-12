CREATE PROCEDURE [GetTaskByEventType]
	@EventName nvarchar(128)
WITH ENCRYPTION
AS
	SELECT a.[ID], a.[EventID], a.[TaskID], a.[Params], a.[IsAllowed]
	FROM [AutomaticallyTasks] AS a
	INNER JOIN [EventTypes] AS e ON e.[ID] = a.[EventID]
	WHERE e.[EventName] = @EventName