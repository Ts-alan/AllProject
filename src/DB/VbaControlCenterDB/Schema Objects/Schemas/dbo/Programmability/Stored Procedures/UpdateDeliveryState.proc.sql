CREATE PROCEDURE [UpdateDeliveryState]
	@Date datetime
WITH ENCRYPTION
AS
	-- Retrieving StateID
	DECLARE @StateDelivery smallint
	DECLARE @StateDeliveryTimeOut smallint
	EXEC @StateDelivery = dbo.GetTaskStateID 'Delivery', 1
	EXEC @StateDeliveryTimeOut = dbo.GetTaskStateID 'Delivery timeout', 1

	UPDATE [Tasks]
	SET	[StateID] = @StateDeliveryTimeOut,
	            [DateUpdated] = GETDATE()
	WHERE [StateID] = @StateDelivery AND [DateIssued] < @Date