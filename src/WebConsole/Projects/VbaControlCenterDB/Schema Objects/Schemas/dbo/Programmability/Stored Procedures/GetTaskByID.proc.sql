CREATE PROCEDURE [GetTaskByID]
	@ID bigint
WITH ENCRYPTION
AS
	SELECT t.[ID], t.[ComputerID], t.[StateID], t.[DateComplete], t.[DateUpdated], t.[TaskParams], tt.[TaskName], t.[TaskUser] 
	FROM [Tasks] as t
	INNER JOIN [TaskTypes] as tt ON t.[TaskID] = tt.[ID]
	WHERE t.[ID] = @ID