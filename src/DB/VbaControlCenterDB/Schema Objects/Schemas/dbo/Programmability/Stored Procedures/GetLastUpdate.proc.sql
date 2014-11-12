CREATE PROCEDURE [dbo].[GetLastUpdate]
	@State nvarchar(32)
WITH ENCRYPTION
AS
	SELECT TOP 1 ul.BuildId, ul.DeployDatetime, us.[State], ul.[Description] FROM UpdateLog AS ul
	INNER JOIN UpdateStates AS us ON us.ID = ul.StateID
	WHERE us.[State] = @State
	ORDER BY ul.DeployDatetime DESC