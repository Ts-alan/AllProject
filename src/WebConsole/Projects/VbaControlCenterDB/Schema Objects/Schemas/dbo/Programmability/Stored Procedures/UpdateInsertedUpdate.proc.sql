CREATE PROCEDURE [dbo].[UpdateInsertedUpdate]
	@BuildId nvarchar(50),
	@DeployDatetime smalldatetime,
	@State nvarchar(32),
	@Description nvarchar(256) = NULL
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT ID FROM UpdateStates WHERE [State] = @State)

	UPDATE UpdateLog
	SET BuildId = @BuildId,
		StateID = @StateID,
		[Description] = @Description
	WHERE DeployDatetime = @DeployDatetime