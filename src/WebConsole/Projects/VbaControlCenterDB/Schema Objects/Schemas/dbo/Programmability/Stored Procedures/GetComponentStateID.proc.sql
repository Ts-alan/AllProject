CREATE PROCEDURE [GetComponentStateID]
	@ComponentState nvarchar(32),
	@InsertIfNotExists tinyint = 0
WITH ENCRYPTION
AS
BEGIN
	IF @InsertIfNotExists = 1
	BEGIN
		-- Checking whether there exists such a component state
		IF NOT EXISTS (SELECT [ID] FROM [ComponentStates] WHERE [ComponentState] = @ComponentState)
			INSERT INTO [ComponentStates](ComponentState) VALUES (@ComponentState);
	END
	RETURN (SELECT [ID] FROM [ComponentStates] WHERE [ComponentState] = @ComponentState)
END