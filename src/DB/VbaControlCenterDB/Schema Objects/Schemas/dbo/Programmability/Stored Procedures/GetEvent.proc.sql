CREATE PROCEDURE [dbo].[GetEvent]
	@ID int
WITH ENCRYPTION
AS
	SELECT
		[ID],
		[ComputerID],
		[EventID],
		[EventTime],
		[ComponentID],
		[Object],
		[Comment]
	FROM
		[dbo].[Events]
	WHERE
		[ID] = @ID