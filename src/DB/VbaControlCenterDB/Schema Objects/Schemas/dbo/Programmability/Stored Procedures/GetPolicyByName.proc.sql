CREATE PROCEDURE [dbo].[GetPolicyByName]
	@TypeName nvarchar(128)

WITH ENCRYPTION
AS
	SELECT [ID], [TypeName], [Params], [Comment] FROM [PolicyTypes]
	WHERE [TypeName] = @TypeName