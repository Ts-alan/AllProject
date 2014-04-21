CREATE PROCEDURE [UpdateComponentState]
	@ComputerName nvarchar(64),
	@ComponentName nvarchar(64),
	@ComponentState nvarchar(32),
	@Version nvarchar(64) = NULL
WITH ENCRYPTION
AS
	-- Retrieving ComputerID
	DECLARE @ComputerID smallint
	EXEC @ComputerID = dbo.GetComputerID @ComputerName
	IF @ComputerID = 0
	BEGIN
		RAISERROR(N'Unable to find computer %s', 16, 1, @ComputerName)
		RETURN
	END
	
	-- Retrieving ComponentID
	DECLARE @ComponentID smallint
	EXEC @ComponentID = dbo.GetComponentTypeID @ComponentName, 1
	
	-- Retrieving StateID
	DECLARE @StateID smallint
	EXEC @StateID = dbo.GetComponentStateID @ComponentState, 1
	
	-- Inserting/updating data
	IF NOT EXISTS (SELECT [ComputerID] FROM [Components] WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID)
	BEGIN
		-- Insert
		INSERT INTO [Components](ComputerID, ComponentID, StateID, Version)
			VALUES(@ComputerID, @ComponentID, @StateID, @Version)
	END
	ELSE
	BEGIN
		-- Update
		UPDATE [Components]
		SET	[ComputerID] = @ComputerID,
			[ComponentID] = @ComponentID,
			[StateID] = @StateID
		WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID
		-- Updating version if necessary
		IF @Version IS NOT NULL
			UPDATE [Components]
			SET	[Version] = @Version
			WHERE [ComputerID] = @ComputerID AND [ComponentID] = @ComponentID
	END