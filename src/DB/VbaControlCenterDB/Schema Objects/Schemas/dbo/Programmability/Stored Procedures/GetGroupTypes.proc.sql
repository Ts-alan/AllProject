CREATE PROCEDURE [dbo].[GetGroupTypes]
WITH ENCRYPTION
AS
	SELECT * FROM [dbo].[GroupTypes]
	ORDER BY [GroupName] ASC