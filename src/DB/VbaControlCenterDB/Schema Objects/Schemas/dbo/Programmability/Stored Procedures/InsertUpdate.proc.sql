CREATE PROCEDURE [dbo].[InsertUpdate]
	@State nvarchar(32)
WITH ENCRYPTION
AS
	DECLARE @StateID smallint
	SET @StateID = (SELECT ID FROM UpdateStates WHERE [State] = @State)

	INSERT INTO UpdateLog (BuildId, DeployDatetime, StateID)
	VALUES ('', GETDATE(), @StateID)