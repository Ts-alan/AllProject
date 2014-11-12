CREATE PROCEDURE [dbo].[GetEventType]
	@ID smallint
WITH ENCRYPTION
AS
	SELECT
		[ID],
		[EventName],
		[Color]
	FROM
		[dbo].[EventTypes]
	WHERE
		[ID] = @ID