CREATE PROCEDURE [IsRunningTask]
	@ComputerName nvarchar(64),
	@TaskID smallint
WITH ENCRYPTION
AS
	SELECT ts.[TaskState], DATEDIFF(minute, t.[DateIssued], GetDate())
	FROM Tasks AS t
	INNER JOIN Computers AS c ON c.[ID] = t.[ComputerID]
	INNER JOIN TaskStates AS ts ON ts.[ID] = t.[StateID]
	WHERE c.[ComputerName] = @ComputerName 
	AND t.[TaskID] = @TaskID 
	AND t.[DateIssued] = (SELECT MAX(Tasks.[DateIssued]) 
							FROM Tasks 
							INNER JOIN Computers ON Computers.[ID] = Tasks.[ComputerID]
							INNER JOIN TaskStates ON TaskStates.[ID] = Tasks.[StateID]
							WHERE Computers.[ComputerName] = @ComputerName AND Tasks.[TaskID] = @TaskID )