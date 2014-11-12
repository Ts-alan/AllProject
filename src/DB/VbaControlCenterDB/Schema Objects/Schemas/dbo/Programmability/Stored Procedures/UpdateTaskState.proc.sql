CREATE PROCEDURE [UpdateTaskState]
	@TaskID bigint,
	@TaskState nvarchar(32),
	@Date datetime,
	@Description nvarchar(256) = NULL
WITH ENCRYPTION
AS
	-- Retrieving StateID
	DECLARE @StateID smallint
	EXEC @StateID = dbo.GetTaskStateID @TaskState, 1

	DECLARE @Priority1 smallint
	DECLARE @Priority2 smallint
	
	IF @TaskState IN (N'Completed successfully', N'Execution error', N'Stopped',
					  N'Delivery timeout', N'Execution timeout', N'Error')
			SET @Priority1 = 4
	ELSE
		IF @TaskState IN (N'Execution')
			SET @Priority1 = 3
		ELSE
			IF @TaskState IN (N'Sended')
				SET @Priority1 = 2
			ELSE
				IF @TaskState IN (N'In queue')
					SET @Priority1 = 1
				ELSE
					IF @TaskState IN (N'Delivery')
						SET @Priority1 = 0

	
	DECLARE @TaskStateOld nvarchar(32)
	SET @TaskStateOld = (SELECT ts.[TaskState] FROM Tasks AS t
						 INNER JOIN TaskStates AS ts ON t.[StateID] = ts.[ID]
						 WHERE t.[ID] = @TaskID)
	IF @TaskStateOld IN (N'Completed successfully', N'Execution error', N'Stopped',
					  N'Delivery timeout', N'Execution timeout', N'Error')
			SET @Priority2 = 4
	ELSE
		IF @TaskStateOld IN (N'Execution')
			SET @Priority2 = 3
		ELSE
			IF @TaskStateOld IN (N'Sended')
				SET @Priority2 = 2
			ELSE
				IF @TaskStateOld IN (N'In queue')
					SET @Priority2 = 1
				ELSE
					IF @TaskStateOld IN (N'Delivery')
						SET @Priority2 = 0

	IF(@Priority2 < @Priority1)
	BEGIN
	
		UPDATE [Tasks]
		SET	[StateID] = @StateID,
			[DateUpdated] = @Date
		WHERE [ID] = @TaskID

		IF @Description IS NOT NULL
		BEGIN
			UPDATE [Tasks]
				SET	[TaskDescription] = @Description
				WHERE [ID] = @TaskID
		END
		
		-- Checking particular states
		IF @TaskState = N'Completed successfully'
		BEGIN
			UPDATE [Tasks]
			SET	[DateComplete] = @Date
			WHERE [ID] = @TaskID

			--костыль для удаления компа после успешного завершения задачи на отсоединение агента
			IF (SELECT tt.[TaskName] FROM Tasks AS t
				INNER JOIN TaskTypes AS tt ON t.[TaskID] = tt.[ID]
				WHERE t.[ID] = @TaskID) IN ('Detach agent', 'Отсоединить агент')
			BEGIN
				DECLARE @ComputerID smallint
				SET @ComputerID = (SELECT [ComputerID] FROM [Tasks] WHERE [ID] = @TaskID)
				
				-- установить статус "Неуправляемый"
				UPDATE ComputerAdditionalInfo
				SET [IsControllable] = 0
				WHERE [ComputerID] = @ComputerID

				-- почистить таблицы
				DELETE FROM DevicesPolicies
				WHERE [ComputerID] = @ComputerID

				DELETE FROM Groups
				WHERE [ComputerID] = @ComputerID
			END
		END	
	END

	-- Recent activity time	
	IF(@Priority1 > 2 AND @TaskState != N'Error')
	BEGIN
		UPDATE [Computers]
		SET [RecentActive] = GETDATE()
		WHERE [ID] = (SELECT ComputerID FROM [Tasks] WHERE [ID] = @TaskID)
	END